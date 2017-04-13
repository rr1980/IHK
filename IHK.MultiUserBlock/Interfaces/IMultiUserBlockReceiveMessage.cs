using IHK.Common;

namespace IHK.MultiUserBlock.Interfaces
{
    public interface IMultiUserBlockReceiveMessage
    {
        string Command { get; set; }
        EntityType EntityType { get; set; }
        string SocketId { get; set; }
        int UserId { get; set; }
        int EntityId { get; set; }
    }
}
