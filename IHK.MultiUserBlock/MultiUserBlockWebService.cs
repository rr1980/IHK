using System;
using System.Collections.Generic;
using System.Text;
using IHK.Common;
using System.Linq;
using IHK.ViewModels;
using IHK.Services;
using System.Threading.Tasks;

namespace IHK.MultiUserBlock
{
    public class MultiUserBlockWebService
    {
        private readonly AccountService _accountService;

        public MultiUserBlockWebService(AccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<MUBBlockViewModel> Request(EntityType entityType, int entityId, int userId, string description)
        {

            var blocks = MultiUserBlockManager.GetBlocksBy(c => c.EntityType == entityType && c.EntityId == entityId);

            if (blocks.Any())
            {
                var selfblocks = blocks.FirstOrDefault(c => c.UserId == userId);
                if(selfblocks != null)
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
                return await Map(MultiUserBlockManager.AddToBlock(Guid.NewGuid().ToString(),entityType, entityId, userId, description));
            }
        }

        private List<MUBBlock> _getWaits(MUBBlock block)
        {
            return MultiUserBlockManager.GetBlocksBy(c => c.EntityType == block.EntityType && c.EntityId == block.EntityId);
        }

        internal async Task<MUBBlockViewModel> Map(MUBBlock block)
        {
            if (block.Position == 0)
            {
                return new MUBBlockViewModel()
                {
                    Description=block.Description,
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

                return new MUBBlockViewModel()
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
                    Waits = waits.Select(s => new MUBBlockViewModel()
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
    }
}
