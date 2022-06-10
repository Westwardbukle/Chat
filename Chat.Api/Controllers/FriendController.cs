using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Chat.Core.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Authorize]
    [Route("/api/v{version:apiVersion}/request")]
    public class FriendController : ControllerBase
    {
        private readonly IFriendService _friendService;

        public FriendController(IFriendService friendService)
        {
            _friendService = friendService;
        }


        
        
        /// <summary>
        /// Confirm request
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPut("{requestId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> ConfirmationRequest([Required]Guid requestId)
        {
            var request =   await _friendService.ConfirmFriendRequest(requestId);

             return Ok(request);
        }
    }
}