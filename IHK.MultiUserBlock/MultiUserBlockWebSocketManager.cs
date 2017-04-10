using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using IHK.Common;
using System.Collections.Generic;
using System.Diagnostics;

namespace IHK.MultiUserBlock
{
    public class Connection
    {
        public WebSocket ws { get; set; }
        public string SocketId { get; set; }
        public int UserId { get; set; }
        //public int EntityId { get; set; }
        //public EntityType EntityType { get; set; }
        //public int Position { get; set; }
    }

    public class Block
    {
        public EntityType EntityType { get; set; }
        public int EntityId { get; set; }
        public int UserId { get; set; }
        //public int Position { get; set; }
    }

    public class MultiUserBlockWebSocketManager
    {
        private static List<Connection> _cons = new List<Connection>();
        private static List<Block> _blocks = new List<Block>();
        private static object _lockerBlock = new object();
        private static object _lockerCons = new object();
        //private ConcurrentDictionary<int, ConcurrentDictionary<string, WebSocket>> _sockets = new ConcurrentDictionary<int, ConcurrentDictionary<string, WebSocket>>();
        //private ConcurrentDictionary<EntityType, ConcurrentDictionary<int, List<int>>> _blocks = new ConcurrentDictionary<EntityType, ConcurrentDictionary<int, List<int>>>();

        public WebSocket GetSocketById(string socketId)
        {
            return _cons.FirstOrDefault(c => c.SocketId == socketId).ws;
        }

        public List<Connection> GetAll()
        {
            return _cons;
        }

        public string GetSocketId(WebSocket socket)
        {
            return _cons.FirstOrDefault(c => c.ws == socket).SocketId;
        }

        public void AddSocket(WebSocket socket, int userId)
        {
            var result = _cons.FirstOrDefault(c => c.ws == socket && c.UserId == userId);

            if (result != null)
            {
                return;
            }

            var con = new Connection()
            {
                ws = socket,
                UserId = userId,
                SocketId = CreateConnectionId()
            };
            lock(_lockerCons)
            {
                _cons.Add(con);
            }
        }

        public async Task RemoveSocket(string socketId, int userId)
        {
            var con = _cons.FirstOrDefault(c => c.SocketId == socketId && c.UserId == userId);
            lock (_lockerCons)
            {
                _cons.Remove(con);
            }
            lock (_lockerBlock)
            {
                _blocks.RemoveAll(b => b.UserId == userId);
            }
            await con.ws.CloseAsync(closeStatus: WebSocketCloseStatus.NormalClosure,
                              statusDescription: "Closed by the MultiUserBlockWebSocketManager",
                              cancellationToken: CancellationToken.None);
        }

        private string CreateConnectionId()
        {
            return Guid.NewGuid().ToString();
        }

        public int AddBlock(EntityType entityType, int entityId, int userId)
        {
            List<Block> blocks;
            lock (_lockerBlock)
            {
                blocks = _blocks.Where(b => b.EntityId == entityId && b.EntityType == entityType).ToList();
            }
            var block = blocks.FirstOrDefault(b => b.UserId == userId);

            //if (blocks.Any())
            //{
            if (block == null)
            {
                block = new Block()
                {
                    EntityType = entityType,
                    EntityId = entityId,
                    UserId = userId
                };
                lock (_lockerBlock)
                {
                    _blocks.Add(block);
                }
            }


            //}
            //else
            //{
            //_blocks.Add(new Block()
            //{
            //    EntityType = entityType,
            //    EntityId = entityId,
            //    UserId = userId
            //});
            //}

            return _blocks.IndexOf(block);
        }
    }
}
