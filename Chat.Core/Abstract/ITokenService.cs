﻿using System;
using Chat.Common.Dto.Token;
using Chat.Common.Dto.User;

namespace Chat.Core.Abstract
{
    public interface ITokenService
    {
        TokenModel CreateToken(UserModelDto user);
        public Guid GetCurrentUserId();
    }
}