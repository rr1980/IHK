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

namespace IHK.MultiUserBlock
{
    public static class MultiUserBlockManager
    {
        internal static readonly List<MultiUserBlockItem> _blocks = new List<MultiUserBlockItem>();
        internal static IMultiUserBlockWebService _multiUserBlockWebService;

        private static readonly object _locker = new object();
        private static readonly Timer _updateTimer;
        private static readonly JsonSerializerSettings _jsonsettings;

        static MultiUserBlockManager()
        {
            _jsonsettings = new JsonSerializerSettings()
            {
                ContractResolver = new LowercaseContractResolver()
            };

            _updateTimer = new Timer(_checkBlocks, null, 0, 1000);
        }

        internal static void OnConnected(WebSocket socket, int userId)
        {
            Debug.WriteLine("OnConnected");
        }

        internal static async Task ReceiveAsync(WebSocket socket, MultiUserBlockReceiveMessage msg)
        {
            var block = _blocks.FirstOrDefault(c => c.EntityType == msg.EntityType && c.EntityId == msg.EntityId && c.UserId == msg.UserId);

            if (block == null)
            {
                block = AddToBlock(Guid.NewGuid().ToString(), msg.EntityType, msg.EntityId, msg.UserId, null);
            }

            _setSocketToBlock(socket, block);

            if (msg.Command == MultiUserBlockCommand.Ping)
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

        internal static async Task OnDisconnected(WebSocket socket, int userId)
        {
            Debug.WriteLine("OnDisconnected");

            await socket.CloseAsync(closeStatus: WebSocketCloseStatus.NormalClosure,
                          statusDescription: "Closed by the MultiUserBlockWebSocketManager",
                          cancellationToken: CancellationToken.None);
        }

        public static async Task SendMessageAsync(WebSocket socket, string message)
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

        internal static List<MultiUserBlockItem> GetBlocksBy(Func<MultiUserBlockItem, bool> p)
        {
            return _blocks.Where(p).ToList();
        }

        internal static MultiUserBlockItem AddToBlock(string id, EntityType entityType, int entityId, int userId, string description = "")
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
                _blocks.Add(block);
            }

            var blocks = MultiUserBlockManager.GetBlocksBy(c => c.EntityType == entityType && c.EntityId == entityId);

            if (string.IsNullOrEmpty(block.Description) && (blocks.Any()))
            {
                block.Description = blocks.First().Description;
            }

            block.Position = blocks.IndexOf(block);
            _updateBlocks(block);

            return block;
        }

        private static void _setSocketToBlock(WebSocket socket, MultiUserBlockItem block)
        {
            if (block != null && (block.Socket == null))
            {
                lock (block)
                {
                    block.Socket = socket;
                }
            }
        }

        private static void _checkBlocks(object state)
        {
            List<MultiUserBlockItem> todelete = new List<MultiUserBlockItem>();
            foreach (var block in _blocks.Where(b => b.Init == false))
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
                    _blocks.Remove(block);
                    _updateBlocks(block);
                    Debug.WriteLine("DELETE: " + block.UserId);
                }
            }
        }

        private static async void _updateBlocks(MultiUserBlockItem block)
        {
            var all = _blocks.Where(w => w.EntityId == block.EntityId && w.EntityType == block.EntityType).Where(s => s.Socket != null).ToList();
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

        private static async void _update(MultiUserBlockItem block)
        {
            var vm = await _multiUserBlockWebService.Map(block);
            string message = JsonConvert.SerializeObject(vm, Formatting.Indented, _jsonsettings);

            Debug.WriteLine("SEND COMMAND: " + block.Command.ToString());
            if (block.Socket.State == WebSocketState.Open)
            {
                await SendMessageAsync(block.Socket, message);
            }
        }
    }


}
