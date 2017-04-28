﻿using IHK.Common;
using IHK.MultiUserBlock.Interfaces;
using System;
using System.Net.WebSockets;

namespace IHK.MultiUserBlock
{
    public class MultiUserBlockItem : IMultiUserBlockItem
    {
        public bool Active { get; set; } = false;
        public Enum Command { get; set; }
        public WebSocket Socket { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool Init { get; set; } = true;
        public string Description { get; set; }
        public string SocketId { get; set; }
        public EntityType EntityType { get; set; }
        public int UserId { get; set; }
        public int EntityId { get; set; }
        public int Position { get; set; }
    }
}
