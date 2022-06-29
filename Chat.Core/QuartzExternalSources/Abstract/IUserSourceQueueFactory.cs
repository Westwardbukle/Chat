using System.Collections.Generic;

namespace Chat.Core.QuartzExternalSources.Abstract
{
    public interface IUserSourceQueueFactory
    {
        Queue<IUserSource>  CreateUserSourceQueue();
    }
}