using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Chat.Common.Dto.Token;
using Chat.Core.Options;
using Microsoft.IdentityModel.Tokens;

namespace Chat.Core.Services
{
    public class TokenService
    {
        private readonly string _secretKey;

        public TokenService
        (
            AppOptions appOptions
        )
        {
            _secretKey = appOptions.SecretKey;
        }
        
        /*private TokenModel BuildTokenModel()
        {
            var handler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);
            var tokenExpiration = DateTime.Now.AddDays(365);

            var securityToken = new JwtSecurityToken
            (
                claims: ,
                expires: tokenExpiration,
                notBefore: DateTime.Now,
                signingCredentials:
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            );

            var token = handler.WriteToken(securityToken);

            return 
        }*/
    }
}