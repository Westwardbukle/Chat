﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Chat.Database.Model;

namespace Chat.Core.ExternalSources.Abstract
{
    public interface IUserApi
    {
        Task<IEnumerable<UserModel>> SendRequest();
    }
}