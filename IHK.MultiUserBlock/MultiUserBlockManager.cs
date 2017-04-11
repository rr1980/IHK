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

namespace IHK.MultiUserBlock
{

    public class ReceiveMsg
    {
        public MUBSocketCommand Command { get; set; }
        public EntityType EntityType { get; set; }
        public string SocketId { get; set; }
        public int UserId { get; set; }
        public int EntityId { get; set; }
    }

    public class MUBBlock
    {
        public MUBSocketCommand Command { get; set; }
        public WebSocket Socket { get; set; }
        public string SocketId { get; set; }
        public EntityType EntityType { get; set; }
        public int UserId { get; set; }
        public int EntityId { get; set; }
        public int Position { get; set; }
        //public List<MUBBlock> Waits { get; set; } = new List<MUBBlock>();
    }

    public static class MultiUserBlockManager
    {
        //internal static List<SocketConnection> _connections = new List<SocketConnection>();
        internal static List<MUBBlock> _blocks = new List<MUBBlock>();
        private static object _locker = new object();
        private static readonly MultiUserBlockWebService _multiUserBlockWebService = new MultiUserBlockWebService();



        internal static void OnConnected(WebSocket socket, int userId)
        {
            Debug.WriteLine("OnConnected");
            //var block = MultiUserBlockManager.GetBlocksBy(c => c.EntityType == entityType && c.EntityId == entityId);
            //SocketConnection sc = new SocketConnection()
            //{
            //    Socket = socket,
            //    SocketId = Guid.NewGuid().ToString(),
            //    UserId = userId
            //};
            //lock (_locker)
            //{
            //    _connections.Add(sc);
            //}
            //_print(sc, "connected as " + sc.SocketId);

            //return sc;
        }

        internal static async Task ReceiveAsync(WebSocket socket, ReceiveMsg msg)
        {

            if (socket.State != WebSocketState.Open)
                return;

            Debug.WriteLine("ReceiveAsync");

            if (msg.Command == MUBSocketCommand.Block)
            {
                var block = _blocks.FirstOrDefault(c => c.EntityType == msg.EntityType && c.EntityId == msg.EntityId && c.UserId == msg.UserId && c.SocketId == msg.SocketId);
                //var block = _findBlockInBlocks(_blocks, msg.EntityType, msg.EntityId, msg.UserId);
                if (block != null)
                {
                    lock (block)
                    {
                        block.Socket = socket;
                    }
                }

            }


            //await socketConnection.Socket.SendAsync(buffer: new ArraySegment<byte>(array: Encoding.ASCII.GetBytes(message),
            //                                                      offset: 0,
            //                                                      count: message.Length),
            //                       messageType: WebSocketMessageType.Text,
            //                       endOfMessage: true,
            //                       cancellationToken: CancellationToken.None);

            //await SendMessageToAllAsync(message);
            //_print(socketConnection, "Msg: " + msg);
        }



        internal static async Task OnDisconnected(WebSocket socket, int userId)
        {
            var block = _blocks.FirstOrDefault(c => c.Socket == socket && c.UserId == userId);
            if (block != null)
            {
                lock (_locker)
                {
                    _blocks.Remove(block);
                }

                var all = _blocks.Where(w => w.EntityId == block.EntityId && w.EntityType == block.EntityType).Where(s => s != null).ToList();

                foreach (var so in all)
                {
                    //if(so.Socket != null)
                    so.Position = all.IndexOf(so);
                    so.Command = MUBSocketCommand.Update;
                    var message = JsonConvert.SerializeObject(so);
                    if (so.Socket.State == WebSocketState.Open)
                        await SendMessageAsync(so.Socket, message);
                }
            }

            await socket.CloseAsync(closeStatus: WebSocketCloseStatus.NormalClosure,
                                      statusDescription: "Closed by the MultiUserBlockWebSocketManager",
                                      cancellationToken: CancellationToken.None);
            Debug.WriteLine("OnDisconnected");

            //_print(socketConnection, "disconnected as " + socketConnection.SocketId);

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

        private static MUBBlock _findBlockInBlocks(List<MUBBlock> blocks, WebSocket socket, int userId)
        {
            return blocks.FirstOrDefault(b => b.Socket == socket && b.UserId == userId);
        }

        private static MUBBlock _findBlockInBlocks(List<MUBBlock> blocks, EntityType entityType, int entityId, int userId)
        {
            return blocks.FirstOrDefault(c => c.EntityType == entityType && c.EntityId == entityId && c.UserId == userId);
        }

        //private static void _print(SocketConnection socketConnection, string msg)
        //{
        //    var message = $"{socketConnection.UserId} : {msg}";
        //    Debug.WriteLine(message);
        //}

        internal static List<MUBBlock> GetBlocksBy(Func<MUBBlock, bool> p)
        {
            return _blocks.Where(p).ToList();
        }

        internal static MUBBlock AddToBlock(string id, EntityType entityType, int entityId, int userId)
        {
            var block = new MUBBlock()
            {
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
            block.Position = blocks.IndexOf(block);

            return block;
        }

        //internal static void AddToBlock(EntityType entityType, int entityId, int userId)
        //{
        //    AddToBlock(_blocks, entityType, entityId, userId);
        //}
    }


    //public class MultiUserBlockWebSocketManager
    //{
    //    private static List<Connection> _cons = new List<Connection>();
    //    private static List<Block> _blocks = new List<Block>();
    //    private static object _lockerBlock = new object();
    //    private static object _lockerCons = new object();
    //    //private ConcurrentDictionary<int, ConcurrentDictionary<string, WebSocket>> _sockets = new ConcurrentDictionary<int, ConcurrentDictionary<string, WebSocket>>();
    //    //private ConcurrentDictionary<EntityType, ConcurrentDictionary<int, List<int>>> _blocks = new ConcurrentDictionary<EntityType, ConcurrentDictionary<int, List<int>>>();

    //    public WebSocket GetSocketById(string socketId)
    //    {
    //        return _cons.FirstOrDefault(c => c.SocketId == socketId).ws;
    //    }

    //    public List<Connection> GetAll()
    //    {
    //        return _cons;
    //    }

    //    public string GetSocketId(WebSocket socket)
    //    {
    //        return _cons.FirstOrDefault(c => c.ws == socket).SocketId;
    //    }

    //    public void AddSocket(WebSocket socket, int userId)
    //    {
    //        var result = _cons.FirstOrDefault(c => c.ws == socket && c.UserId == userId);

    //        if (result != null)
    //        {
    //            return;
    //        }

    //        var con = new Connection()
    //        {
    //            ws = socket,
    //            UserId = userId,
    //            SocketId = CreateConnectionId()
    //        };
    //        lock(_lockerCons)
    //        {
    //            _cons.Add(con);
    //        }
    //    }

    //    public async Task RemoveSocket(string socketId, int userId)
    //    {
    //        var con = _cons.FirstOrDefault(c => c.SocketId == socketId && c.UserId == userId);
    //        lock (_lockerCons)
    //        {
    //            _cons.Remove(con);
    //        }
    //        lock (_lockerBlock)
    //        {
    //            _blocks.RemoveAll(b => b.UserId == userId);
    //        }
    //        await con.ws.CloseAsync(closeStatus: WebSocketCloseStatus.NormalClosure,
    //                          statusDescription: "Closed by the MultiUserBlockWebSocketManager",
    //                          cancellationToken: CancellationToken.None);
    //    }

    //    private string CreateConnectionId()
    //    {
    //        return Guid.NewGuid().ToString();
    //    }

    //    public int AddBlock(EntityType entityType, int entityId, int userId)
    //    {
    //        List<Block> blocks;
    //        lock (_lockerBlock)
    //        {
    //            blocks = _blocks.Where(b => b.EntityId == entityId && b.EntityType == entityType).ToList();
    //        }
    //        var block = blocks.FirstOrDefault(b => b.UserId == userId);

    //        //if (blocks.Any())
    //        //{
    //        if (block == null)
    //        {
    //            block = new Block()
    //            {
    //                EntityType = entityType,
    //                EntityId = entityId,
    //                UserId = userId
    //            };
    //            lock (_lockerBlock)
    //            {
    //                _blocks.Add(block);
    //            }
    //        }


    //        //}
    //        //else
    //        //{
    //        //_blocks.Add(new Block()
    //        //{
    //        //    EntityType = entityType,
    //        //    EntityId = entityId,
    //        //    UserId = userId
    //        //});
    //        //}

    //        return _blocks.IndexOf(block);
    //    }
    //}
}
