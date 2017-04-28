using IHK.Common;
using IHK.Common.MultiUserBlockCommon;
using IHK.MultiUserBlock.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace IHK.MultiUserBlock
{
    /// <summary>
    /// Klasse des öffentlichen MultiUserBlock-Service für AspNetCore dependency injection
    /// </summary>
    public class MultiUserBlockWebService : IMultiUserBlockWebService
    {
        private readonly IMultiUserBlockManager _multiUserBlockManager;
        private readonly ILogger<MultiUserBlockManager> _logger;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="multiUserBlockManager"></param>
        public MultiUserBlockWebService(IMultiUserBlockManager multiUserBlockManager, ILogger<MultiUserBlockManager> logger)
        {
            _multiUserBlockManager = multiUserBlockManager;
            _logger = logger;

        }

        /// <summary>
        /// Methode um Entityblockierung einzuleiten
        /// </summary>
        /// <param name="entityType">Type des Entitys</param>
        /// <param name="entityId">Eindeutige ID der Entity</param>
        /// <param name="userId">Eindeutige ID des Benutzers</param>
        /// <param name="description">Beschreibung der Entity</param>
        /// <returns>Liefert "IMultiUserBlockViewModel" mit Blockierungsdaten zurück</returns>
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
                    _logger.LogWarning("User: " + userId + " request: " + entityType.ToString() + ": " + entityId + " again");
                    return await _multiUserBlockManager.Map(selfblocks);
                }
                else
                {
                    _logger.LogWarning("User: " + userId + " request: " + entityType.ToString() + ": " + entityId);
                    return await _multiUserBlockManager.Map(_multiUserBlockManager.AddToBlock(Guid.NewGuid().ToString(), entityType, entityId, userId, description));
                }
            }
            else
            {
                _logger.LogWarning("User: " + userId + " request: " + entityType.ToString() + ": " + entityId);
                return await _multiUserBlockManager.Map(_multiUserBlockManager.AddToBlock(Guid.NewGuid().ToString(), entityType, entityId, userId, description));
            }
        }
    }
}
