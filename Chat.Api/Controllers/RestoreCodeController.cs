using System.Threading.Tasks;
using Chat.Common.Dto.Code;
using Chat.Common.Result;
using Chat.Common.User;
using Chat.Core.Restoring;
using Chat.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("/api/v{version:apiVersion}/[controller]")]
    public class RestoreCodeController : BaseController
    {
        private readonly IRestoringCodeService _restoringCode;
        
        public RestoreCodeController
        (
            IRestoringCodeService restoringCode
        )
        {
            _restoringCode = restoringCode;
        }
        
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="codeDto"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<ActionResult> EmailConfirm(CodeDto codeDto)
            => await ReturnResult<ResultContainer<CodeResponseDto>, CodeResponseDto>
                (_restoringCode.CodeСonfirmation(codeDto));


        /// <summary>
        /// sending code
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userDto"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<ActionResult> SendRestoringCode(UserDto userDto)
            => await ReturnResult<ResultContainer<CodeResponseDto>, CodeResponseDto>
                (_restoringCode.SendRestoringCode(userDto));
    }
}