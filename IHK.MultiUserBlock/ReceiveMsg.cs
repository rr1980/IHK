using IHK.Common;

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
}
