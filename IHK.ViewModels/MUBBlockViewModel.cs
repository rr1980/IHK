using IHK.Common;
using System.Collections.Generic;

namespace IHK.ViewModels
{
    public class MUBBlockViewModel
    {
        public MUBSocketCommand Command { get; set; }
        public string SocketId { get; set; }
        public EntityType EntityType { get; set; }
        public int UserId { get; set; }
        public int EntityId { get; set; }
        public int Position { get; set; }
        public string Name { get; set; }
        public string Vorname { get; set; }
        public string Description { get; set; }
        public string Telefon { get; set; }
        public List<MUBBlockViewModel> Waits { get; set; }
    }
}