using System;
using System.Threading.Tasks;
using Chat.Common.Code;
using Chat.Common.Dto.Code;
using Chat.Core.Abstract;
using Chat.Database.Model;
using Chat.Database.Repository.Manager;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Core.Services
{
    public class RestoringCodeServiceService : IRestoringCodeService
    {
        
        private readonly ICodeService _code;
        private readonly ISmtpService _smtpService;
        private readonly ITokenService _tokenService;
        private readonly IRepositoryManager _repository;
        
        public RestoringCodeServiceService
        (
            ICodeService code,
            ISmtpService smtpService,
            ITokenService tokenService,
            IRepositoryManager repository
        )
        {
            _code = code;
            _smtpService = smtpService;
            _tokenService = tokenService;
            _repository = repository;
        }
        
        
        public async Task<ActionResult> SendRestoringCode(Guid userId)
        {
            var user = _repository.User.GetUser(u => u.Id == userId);

            if (user== null)
            {
                return new StatusCodeResult(StatusCodes.Status400BadRequest);
            }

            var lastCode = _repository.Code.GetCode(c => c.Id == user.Id);

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
                
                _repository.Code.CreateCode(newCode);
                
                return new StatusCodeResult(StatusCodes.Status201Created);
            }

            _repository.Code.DeleteCode(lastCode);
            
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
            
             _repository.Code.CreateCode(code);
            
            return new StatusCodeResult(StatusCodes.Status201Created);
        }
        
        public async Task<ActionResult> CodeСonfirmation(CodeDto codeDto)
        {
            var code = _repository.Code.GetCode(c => c.UserId == _tokenService.GetCurrentUserId());

            if (code is null) return new StatusCodeResult(StatusCodes.Status400BadRequest);
            
            if (code.CodePurpose != CodePurpose.ConfirmEmail && code.DateExpiration > DateTime.Now)
            {
                return new StatusCodeResult(StatusCodes.Status400BadRequest);
            }
            
            var user = _repository.User.GetUser(u => u.Id == code.Id); 
            user.Active = true;
            user.DateTimeActivation = DateTime.Now;

            _repository.User.UpdateUser(user);
            
            _repository.Code.DeleteCode(code);
            
            return new StatusCodeResult(StatusCodes.Status201Created);
        }
    }
}