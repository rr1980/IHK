using System.Text;
using System.Threading.Tasks;

namespace IHK.Common.MultiUserBlockCommon
{
    public interface IMultiUserBlockWebService
    {
        Task<IMultiUserBlockViewModel> Request(EntityType entityType, int entityId, int userId, string description);

    }
}
