using IHK.Common;
using IHK.Common.MultiUserBlockCommon;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace IHK.MultiUserBlock.Interfaces
{
    public interface IMultiUserBlockManager
    {
        //int PingTimeOutSec { get; set; }
        List<IMultiUserBlockItem> Blocks { get; }
        void OnConnected(WebSocket socket, int userId);
        Task ReceiveAsync(WebSocket socket, IMultiUserBlockReceiveMessage msg);
        Task OnDisconnected(WebSocket socket, int userId);
        List<IMultiUserBlockItem> GetBlocksBy(Func<IMultiUserBlockItem, bool> p);
        IMultiUserBlockItem AddToBlock(string id, EntityType entityType, int entityId, int userId, string description = "");
        Task<IMultiUserBlockViewModel> Map(IMultiUserBlockItem block);
    }
}
