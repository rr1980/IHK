using IHK.Common;
using IHK.Common.MultiUserBlockCommon;
using System;
using System.Collections.Generic;

namespace IHK.MultiUserBlock
{
    public class MultiUserBlockViewModel : IMultiUserBlockViewModel
    {
        public Enum Command { get; set; }
        public string SocketId { get; set; }
        public EntityType EntityType { get; set; }
        public int UserId { get; set; }
        public int EntityId { get; set; }
        public int Position { get; set; }
        public string Name { get; set; }
        public string Vorname { get; set; }
        public string Description { get; set; }
        public string Telefon { get; set; }
        public List<IMultiUserBlockViewModel> Waits { get; set; }
    }
}