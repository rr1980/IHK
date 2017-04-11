using IHK.Common;

namespace IHK.ViewModels
{
    public class MUBBlockViewData
    {
        public string SocketId { get; set; }
        public EntityType EntityType { get; set; }
        public int UserId { get; set; }
        public int EntityId { get; set; }
        public int Position { get; set; }
    }
}