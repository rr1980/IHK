using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using IHK.Common;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Newtonsoft.Json;
using System.Linq.Expressions;
using Newtonsoft.Json.Serialization;
using IHK.MultiUserBlock.Interfaces;
using IHK.Common.MultiUserBlockCommon;

namespace IHK.MultiUserBlock
{
    public  class MultiUserBlockManager : IMultiUserBlockManager
    {
        public List<IMultiUserBlockItem> Blocks { get; set; } = new List<IMultiUserBlockItem>();
        
        private readonly IAccountService _accountService;
        private  readonly object _locker = new object();
        private  readonly Timer _updateTimer;
        private  readonly JsonSerializerSettings _jsonsettings;

        public MultiUserBlockManager(IAccountService accountService)
        {
            _accountService = accountService;

            _jsonsettings = new JsonSerializerSettings()
            {
                ContractResolver = new LowercaseContractResolver()
            };

            _updateTimer = new Timer(_checkBlocks, null, 0, 1000);
        }

        public  void OnConnected(WebSocket socket, int userId)
        {
            Debug.WriteLine("OnConnected");
        }

        public async Task ReceiveAsync(WebSocket socket, IMultiUserBlockReceiveMessage msg)
        {
            var block = Blocks.FirstOrDefault(c => c.EntityType == msg.EntityType && c.EntityId == msg.EntityId && c.UserId == msg.UserId);

            if (block == null)
            {
                block = (MultiUserBlockItem)AddToBlock(Guid.NewGuid().ToString(), msg.EntityType, msg.EntityId, msg.UserId, null);
            }

            _setSocketToBlock(socket, block);

            if (msg.Command ==  MultiUserBlockCommand.Ping.ToString())
            {
                block.UpdateTime = DateTime.Now;
                block.Init = false;
                if (block.Position == 0)
                {
                    block.Active = true;
                }
                Debug.WriteLine("PING: " + block.UserId);
            }
        }

        public async Task OnDisconnected(WebSocket socket, int userId)
        {
            Debug.WriteLine("OnDisconnected");

            await socket.CloseAsync(closeStatus: WebSocketCloseStatus.NormalClosure,
                          statusDescription: "Closed by the MultiUserBlockWebSocketManager",
                          cancellationToken: CancellationToken.None);
        }

        public  async Task SendMessageAsync(WebSocket socket, string message)
        {
            if (socket.State != WebSocketState.Open)
                return;

            await socket.SendAsync(buffer: new ArraySegment<byte>(array: Encoding.ASCII.GetBytes(message),
                                                                  offset: 0,
                                                                  count: message.Length),
                                   messageType: WebSocketMessageType.Text,
                                   endOfMessage: true,
                                   cancellationToken: CancellationToken.None);
        }

        public  List<IMultiUserBlockItem> GetBlocksBy(Func<IMultiUserBlockItem, bool> p)
        {
            return Blocks.Where(p).ToList();
        }

        public  IMultiUserBlockItem AddToBlock(string id, EntityType entityType, int entityId, int userId, string description = "")
        {
            var block = new MultiUserBlockItem()
            {
                Description = description,
                SocketId = id,
                EntityType = entityType,
                EntityId = entityId,
                UserId = userId
            };

            lock (_locker)
            {
                Blocks.Add(block);
            }

            var blocks = GetBlocksBy(c => c.EntityType == entityType && c.EntityId == entityId);

            if (string.IsNullOrEmpty(block.Description) && (blocks.Any()))
            {
                block.Description = blocks.First().Description;
            }

            block.Position = blocks.IndexOf(block);
            _updateBlocks(block);

            return block;
        }

        private  void _setSocketToBlock(WebSocket socket, IMultiUserBlockItem block)
        {
            if (block != null && (block.Socket == null))
            {
                lock (block)
                {
                    block.Socket = socket;
                }
            }
        }

        private  void _checkBlocks(object state)
        {
            List<IMultiUserBlockItem> todelete = new List<IMultiUserBlockItem>();
            foreach (var block in Blocks.Where(b => b.Init == false))
            {
                if (block.UpdateTime.AddSeconds(5) < DateTime.Now)
                {
                    todelete.Add(block);
                }
            }

            foreach (var block in todelete)
            {
                lock (_locker)
                {
                    Blocks.Remove(block);
                    _updateBlocks(block);
                    Debug.WriteLine("DELETE: " + block.UserId);
                }
            }
        }

        private  async void _updateBlocks(IMultiUserBlockItem block)
        {
            var all = Blocks.Where(w => w.EntityId == block.EntityId && w.EntityType == block.EntityType).Where(s => s.Socket != null).ToList();
            foreach (var so in all)
            {
                so.Position = all.IndexOf(so);

                if (so.Position == 0)
                {
                    if (so.Command != (Enum)MultiUserBlockCommand.Active)
                    {
                        so.Command = MultiUserBlockCommand.Active;
                        _update(so);
                    }
                }
                else
                {
                    so.Command = MultiUserBlockCommand.Update;
                    _update(so);
                }
            }
        }

        private  async void _update(IMultiUserBlockItem block)
        {
            var vm = await Map(block);
            string message = JsonConvert.SerializeObject(vm, Formatting.Indented, _jsonsettings);

            Debug.WriteLine("SEND COMMAND: " + block.Command.ToString());
            if (block.Socket.State == WebSocketState.Open)
            {
                await SendMessageAsync(block.Socket, message);
            }
        }

        public async Task<IMultiUserBlockViewModel> Map(IMultiUserBlockItem block)
        {
            if (block.Position == 0)
            {
                return new MultiUserBlockViewModel()
                {
                    Description = block.Description,
                    Command = block.Command,
                    SocketId = block.SocketId,
                    EntityType = block.EntityType,
                    UserId = block.UserId,
                    EntityId = block.EntityId,
                    Position = block.Position
                };
            }
            else
            {
                var waits = _getWaits(block);

                return new MultiUserBlockViewModel()
                {
                    Command = block.Command,
                    SocketId = block.SocketId,
                    EntityType = block.EntityType,
                    UserId = block.UserId,
                    EntityId = block.EntityId,
                    Position = block.Position,
                    Name = (await _accountService.GetById(block.UserId)).Name,
                    Vorname = (await _accountService.GetById(block.UserId)).Vorname,
                    Description = block.Description,
                    Telefon = (await _accountService.GetById(block.UserId)).Telefon,
                    Waits = waits.Select(s => (IMultiUserBlockViewModel)new MultiUserBlockViewModel()
                    {
                        Description = block.Description,
                        Command = block.Command,
                        SocketId = s.SocketId,
                        EntityType = s.EntityType,
                        UserId = s.UserId,
                        EntityId = s.EntityId,
                        Position = s.Position,
                        Name = (_accountService.GetById(s.UserId)).Result.Name,
                        Vorname = (_accountService.GetById(s.UserId)).Result.Vorname,
                        Telefon = (_accountService.GetById(s.UserId)).Result.Telefon,
                    }).ToList()
                };
            }
        }
        
        private List<IMultiUserBlockItem> _getWaits(IMultiUserBlockItem block)
        {
            return GetBlocksBy(c => c.EntityType == block.EntityType && c.EntityId == block.EntityId);
        }
    }


}
