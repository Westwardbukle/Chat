using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Chat.Common.Dto.Token;
using Chat.Common.Dto.User;
using Chat.Core.Options;
using Chat.Core.Token;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace Chat.Core.Services
{
    public class TokenService : ITokenService
    {
        private readonly string _secretKey;
        private readonly HttpContext _httpContext;

        public TokenService
        (
            AppOptions appOptions,
            IHttpContextAccessor httpContextAccessor
        )
        {
            _secretKey = appOptions.SecretKey;
            _httpContext = httpContextAccessor.HttpContext;
        }
        
        public TokenModel CreateToken(UserModelDto user)
        {
            var token = BuildTokenModel(user);
            return token;
        }
        
        private TokenModel BuildTokenModel(UserModelDto user)
        {
            var handler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);
            var tokenExpiration = DateTime.UtcNow.Add(TimeSpan.FromMinutes(1));

            var securityToken = new JwtSecurityToken
            (
                claims: GetClaims(user),
                expires: tokenExpiration,
                notBefore: DateTime.UtcNow,
                signingCredentials:
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            );

            var token = handler.WriteToken(securityToken);

            return new TokenModel(token);
        }
        private static IEnumerable<Claim> GetClaims(UserModelDto user)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Email, user.Email),
            };

            return claims;
        }
        public Guid GetCurrentUserId() =>
            Guid.TryParse(_httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId)
                ? userId
                : new Guid();
    }
}