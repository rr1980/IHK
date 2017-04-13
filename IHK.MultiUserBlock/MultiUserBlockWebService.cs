using System;
using System.Collections.Generic;
using System.Text;
using IHK.Common;
using System.Linq;
using System.Threading.Tasks;
using IHK.Common.MultiUserBlockCommon;
using IHK.MultiUserBlock.Interfaces;

namespace IHK.MultiUserBlock
{
    public class MultiUserBlockWebService : IMultiUserBlockWebService
    {
        private readonly IMultiUserBlockManager _multiUserBlockManager;

        public MultiUserBlockWebService(IMultiUserBlockManager multiUserBlockManager)
        {
            _multiUserBlockManager = multiUserBlockManager;
        }

        public async Task<IMultiUserBlockViewModel> Request(EntityType entityType, int entityId, int userId, string description)
        {
            var blocks = _multiUserBlockManager.GetBlocksBy(c => c.EntityType == entityType && c.EntityId == entityId);

            if (blocks.Any())
            {
                var selfblocks = blocks.FirstOrDefault(c => c.UserId == userId);
                if (selfblocks != null)
                {
                    selfblocks.Position = blocks.IndexOf(selfblocks);
                    selfblocks.Description = description;
                    return await _multiUserBlockManager.Map(selfblocks);
                }
                else
                {
                    return await _multiUserBlockManager.Map(_multiUserBlockManager.AddToBlock(Guid.NewGuid().ToString(), entityType, entityId, userId, description));
                }
            }
            else
            {
                return await _multiUserBlockManager.Map(_multiUserBlockManager.AddToBlock(Guid.NewGuid().ToString(), entityType, entityId, userId, description));
            }
        }
    }
}
