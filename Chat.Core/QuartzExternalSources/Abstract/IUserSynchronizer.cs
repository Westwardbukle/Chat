using System.Threading.Tasks;

namespace Chat.Core.QuartzExternalSources.Abstract
{
    public interface IUserSynchronizer
    {
        Task Sync();
    }
}