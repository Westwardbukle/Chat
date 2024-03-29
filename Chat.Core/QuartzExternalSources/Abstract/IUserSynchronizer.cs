﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Chat.Database.Model;

namespace Chat.Core.QuartzExternalSources.Abstract
{
    public interface IUserSynchronizer
    {
        Task Sync();
    }
}