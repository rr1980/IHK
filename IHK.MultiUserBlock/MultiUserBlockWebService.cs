using System;
using System.Collections.Generic;
using System.Text;
using IHK.Common;
using System.Linq;
using System.Threading.Tasks;

namespace IHK.MultiUserBlock
{
    public class MultiUserBlockWebService : IMultiUserBlockWebService
    {
        private readonly IAccountService _accountService;

        public MultiUserBlockWebService(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<IMultiUserBlockViewModel> Request(EntityType entityType, int entityId, int userId, string description)
        {

            var blocks = MultiUserBlockManager.GetBlocksBy(c => c.EntityType == entityType && c.EntityId == entityId);

            if (blocks.Any())
            {
                var selfblocks = blocks.FirstOrDefault(c => c.UserId == userId);
                if (selfblocks != null)
                {
                    selfblocks.Position = blocks.IndexOf(selfblocks);
                    selfblocks.Description = description;
                    return await Map(selfblocks);
                }
                else
                {
                    return await Map(MultiUserBlockManager.AddToBlock(Guid.NewGuid().ToString(), entityType, entityId, userId, description));
                }
            }
            else
            {
                return await Map(MultiUserBlockManager.AddToBlock(Guid.NewGuid().ToString(), entityType, entityId, userId, description));
            }
        }

        public async Task<IMultiUserBlockViewModel> Map(IMultiUserBlockItem block)
        {
            if (block.Position == 0)
            {
                return new MultiUserBlockViewModel()
                {
                    Description = block.Description,
                    Command = block.Command,
                    SocketId = block.SocketId,
                    EntityType = block.EntityType,
                    UserId = block.UserId,
                    EntityId = block.EntityId,
                    Position = block.Position
                };
            }
            else
            {
                var waits = _getWaits(block);

                return new MultiUserBlockViewModel()
                {
                    Command = block.Command,
                    SocketId = block.SocketId,
                    EntityType = block.EntityType,
                    UserId = block.UserId,
                    EntityId = block.EntityId,
                    Position = block.Position,
                    Name = (await _accountService.GetById(block.UserId)).Name,
                    Vorname = (await _accountService.GetById(block.UserId)).Vorname,
                    Description = block.Description,
                    Telefon = (await _accountService.GetById(block.UserId)).Telefon,
                    Waits = waits.Select(s => (IMultiUserBlockViewModel)new MultiUserBlockViewModel()
                    {
                        Description = block.Description,
                        Command = block.Command,
                        SocketId = s.SocketId,
                        EntityType = s.EntityType,
                        UserId = s.UserId,
                        EntityId = s.EntityId,
                        Position = s.Position,
                        Name = (_accountService.GetById(s.UserId)).Result.Name,
                        Vorname = (_accountService.GetById(s.UserId)).Result.Vorname,
                        Telefon = (_accountService.GetById(s.UserId)).Result.Telefon,
                    }).ToList()
                };
            }
        }

        private List<MultiUserBlockItem> _getWaits(IMultiUserBlockItem block)
        {
            return MultiUserBlockManager.GetBlocksBy(c => c.EntityType == block.EntityType && c.EntityId == block.EntityId);
        }
    }
}
