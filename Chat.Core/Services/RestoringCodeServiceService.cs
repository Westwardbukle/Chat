using System;
using System.Threading.Tasks;
using Chat.Common.Code;
using Chat.Common.Dto.Code;
using Chat.Common.Error;
using Chat.Common.Result;
using Chat.Common.User;
using Chat.Core.Code;
using Chat.Core.Restoring;
using Chat.Core.Smtp;
using Chat.Core.Token;
using Chat.Database.Model;
using Chat.Database.Repository.Code;
using Chat.Database.Repository.User;

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
        
        
        public async Task<ResultContainer<CodeResponseDto>> SendRestoringCode(UserDto userDto)
        {
            var result = new ResultContainer<CodeResponseDto>();

            var user = _userRepository.GetOne(u => u.Id == userDto.Userid);

            if (user== null)
            {
                result.ErrorType = ErrorType.BadRequest;
                return result;
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

                result.ErrorType = ErrorType.Create;
                return result;
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

            result.ErrorType = ErrorType.Create;
            return result;
        }
        
        public async Task<ResultContainer<CodeResponseDto>> CodeСonfirmation(CodeDto codeDto)
        {
            var result = new ResultContainer<CodeResponseDto>();
            var code = _codeRepository.GetOne(c => c.UserId == _tokenService.GetCurrentUserId());

            if (code is null)
            {
                result.ErrorType = ErrorType.BadRequest;
                return result;
            }

            if (code.CodePurpose != CodePurpose.ConfirmEmail && code.DateExpiration > DateTime.Now)
            {
                result.ErrorType = ErrorType.BadRequest;
                return result;
            }
            
            var user = _userRepository.GetOne(u => u.Id == code.Id); 
            user.Active = true;
            user.DateTimeActivation = DateTime.Now;

            await _userRepository.Update(user);
            
            await _codeRepository.Delete(code.Id);
            
            result.ErrorType = ErrorType.Create;

            return result;
        }
    }
}