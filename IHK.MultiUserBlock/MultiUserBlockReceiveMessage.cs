using IHK.Common;
using IHK.MultiUserBlock.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace IHK.MultiUserBlock
{
    public class MultiUserBlockReceiveMessage : IMultiUserBlockReceiveMessage
    {
        public string Command { get; set; }
        public EntityType EntityType { get; set; }
        public string SocketId { get; set; }
        public int UserId { get; set; }
        public int EntityId { get; set; }
    }
}
