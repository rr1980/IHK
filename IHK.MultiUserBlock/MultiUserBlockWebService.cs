using System;
using System.Collections.Generic;
using System.Text;
using IHK.Common;
using System.Linq;

namespace IHK.MultiUserBlock
{
    public class MultiUserBlockWebService
    {
        public MUBBlock IsFree(EntityType entityType, int entityId, int userId)
        {

            var blocks = MultiUserBlockManager.GetBlocksBy(c => c.EntityType == entityType && c.EntityId == entityId);

            if (blocks.Any())
            {
                var selfblocks = blocks.FirstOrDefault(c => c.UserId == userId);
                if(selfblocks != null)
                {
                    selfblocks.Position = blocks.IndexOf(selfblocks);
                    return selfblocks;
                }
                else
                {
                    return MultiUserBlockManager.AddToBlock(Guid.NewGuid().ToString(), entityType, entityId, userId);
                }
            }
            else
            {
                return MultiUserBlockManager.AddToBlock(Guid.NewGuid().ToString(),entityType, entityId, userId);
            }
        }

        //internal void AddBlock(SocketConnection socketConnection)
        //{
        //    var blocks = MultiUserBlockManager.GetBlocksBy(c => c.EntityType == socketConnection.EntityType && c.EntityId == socketConnection.EntityId);

        //    if (blocks.Any())
        //    {
        //        var selfblocks = blocks.FirstOrDefault(c => c.UserId == socketConnection.UserId);
        //        if (selfblocks == null)
        //        {
                    
        //        }
        //    }
        //    else
        //    {
        //        MultiUserBlockManager.AddToBlock(new MUBBlock()
        //        {
        //            EntityType = socketConnection.EntityType,
        //            EntityId = socketConnection.EntityId,
        //            UserId = socketConnection.UserId
        //        });
        //    }
        //}
    }
}
