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
            var user = _userRepository.GetOne(u => u.Id == userDto.Userid);

            if (user== null)
            {
                return new StatusCodeResult(StatusCodes.Status400BadRequest);
            }

            var lastCode = _codeRepository.GetOne(c => c.Id == user.Id);

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
                
                await _codeRepository.Create(newCode);
                
                return new StatusCodeResult(StatusCodes.Status201Created);
            }

            await _codeRepository.Delete(lastCode.Id);
            
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
            
            await _codeRepository.Create(code);
            
            return new StatusCodeResult(StatusCodes.Status201Created);
        }
        
        public async Task<ActionResult> CodeСonfirmation(CodeDto codeDto)
        {
            var code = _codeRepository.GetOne(c => c.UserId == _tokenService.GetCurrentUserId());

            if (code is null) return new StatusCodeResult(StatusCodes.Status400BadRequest);
            
            if (code.CodePurpose != CodePurpose.ConfirmEmail && code.DateExpiration > DateTime.Now)
            {
                return new StatusCodeResult(StatusCodes.Status400BadRequest);
            }
            
            var user = _userRepository.GetOne(u => u.Id == code.Id); 
            user.Active = true;
            user.DateTimeActivation = DateTime.Now;

            await _userRepository.Update(user);
            
            await _codeRepository.Delete(code.Id);
            
            return new StatusCodeResult(StatusCodes.Status201Created);
        }
    }
}