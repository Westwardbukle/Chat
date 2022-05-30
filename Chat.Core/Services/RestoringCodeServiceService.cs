using System;
using System.Threading.Tasks;
using Chat.Common.Code;
using Chat.Common.Dto.Code;
using Chat.Common.User;
using Chat.Core.Code;
using Chat.Core.Restoring;
using Chat.Core.Smtp;
using Chat.Core.Token;
using Chat.Database.Model;
using Chat.Database.Repository.Code;
using Chat.Database.Repository.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Core.Services
{
    public class RestoringCodeServiceService : IRestoringCodeService
    {

        private readonly IUserRepository _userRepository;
        private readonly ICodeService _code;
        private readonly ISmtpService _smtpService;
        private readonly ICodeRepository _codeRepository;
        private readonly ITokenService _tokenService;
        
        public RestoringCodeServiceService
        (
            IUserRepository userRepository,
            ICodeService code,
            ISmtpService smtpService,
            ICodeRepository codeRepository,
            ITokenService tokenService
        )
        {
            _userRepository = userRepository;
            _code = code;
            _smtpService = smtpService;
            _codeRepository = codeRepository;
            _tokenService = tokenService;
        }
        
        
        public async Task<ActionResult> SendRestoringCode(UserDto userDto)
        {
            var user = _userRepository.GetUser(u => u.Id == userDto.Userid);

            if (user== null)
            {
                return new StatusCodeResult(StatusCodes.Status400BadRequest);
            }

            var lastCode = _codeRepository.GetCode(c => c.Id == user.Id);

            if (lastCode is null)
            {
                var newMailCode = _code.GenerateRestoringCode();
            
                await _smtpService.SendEmailAsync(user.Email, newMailCode);
                
                var newCode = new CodeModel()
                {
                    Id = user.Id,
                    Code = newMailCode,
                    CodePurpose = CodePurpose.ConfirmEmail,
                    DateCreated = DateTime.Now,
                    DateExpiration = DateTime.Now.AddHours(2),
                    UserId = user.Id
                };
                
                _codeRepository.CreateCode(newCode);
                
                return new StatusCodeResult(StatusCodes.Status201Created);
            }

            _codeRepository.DeleteCode(lastCode);
            
            var mailCode = _code.GenerateRestoringCode();
            
            await _smtpService.SendEmailAsync(user.Email, mailCode);
            
            var code = new CodeModel()
            {
                Id = user.Id,
                Code = mailCode,
                CodePurpose = CodePurpose.ConfirmEmail,
                DateCreated = DateTime.Now,
                DateExpiration = DateTime.Now.AddHours(2),
                UserId = user.Id
            };
            
             _codeRepository.CreateCode(code);
            
            return new StatusCodeResult(StatusCodes.Status201Created);
        }
        
        public async Task<ActionResult> CodeСonfirmation(CodeDto codeDto)
        {
            var code = _codeRepository.GetCode(c => c.UserId == _tokenService.GetCurrentUserId());

            if (code is null) return new StatusCodeResult(StatusCodes.Status400BadRequest);
            
            if (code.CodePurpose != CodePurpose.ConfirmEmail && code.DateExpiration > DateTime.Now)
            {
                return new StatusCodeResult(StatusCodes.Status400BadRequest);
            }
            
            var user = _userRepository.GetUser(u => u.Id == code.Id); 
            user.Active = true;
            user.DateTimeActivation = DateTime.Now;

            _userRepository.UpdateUser(user);
            
            _codeRepository.DeleteCode(code);
            
            return new StatusCodeResult(StatusCodes.Status201Created);
        }
    }
}