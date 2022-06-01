using System.Threading.Tasks;
using Chat.Common.Dto.Code;
using Chat.Common.User;
using Chat.Core.Abstract;
using Chat.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("/api/v{version:apiVersion}/[controller]")]
    public class RestoreCodeController : ControllerBase
    {
        private readonly IRestoringCodeService _restoringCode;
        private readonly ITokenService _tokenService;
        
        public RestoreCodeController
        (
            IRestoringCodeService restoringCode,
            ITokenService tokenService
        )
        {
            _restoringCode = restoringCode;
            _tokenService = tokenService;
        }
        
        
        /// <summary>
        /// Email confirmation code
        /// </summary>
        /// <param name="codeDto"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<ActionResult> EmailСonfirmation(CodeDto codeDto)
            => await _restoringCode.CodeСonfirmation(codeDto);


        /// <summary>
        /// sending code on Email
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userDto"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<ActionResult> RecoveryСode()
        {
            var userId = _tokenService.GetCurrentUserId();
            await _restoringCode.SendRestoringCode(userId);

            return Ok();
        }
        
    }
}