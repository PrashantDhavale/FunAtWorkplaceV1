using FunAtWorkplace.Service.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace FunAtWorkplace.Service.Controllers
{
    [ApiController]
    [Route("telegramsessions")]
    public class TelegramSessionsController : ControllerBase
    {
        #region Members

        private readonly ILogger<TelegramSessionsController> _logger;
        private readonly ITdClientService _iTdClientService;

        #endregion

        #region Constructor

        public TelegramSessionsController(ITdClientService iTdClientService,
                                         ILogger<TelegramSessionsController> logger)
        {
            _iTdClientService = iTdClientService ?? throw new ArgumentNullException(nameof(iTdClientService));
            _logger = logger;
        }

        #endregion

        #region Methods

        [HttpGet]
        [Route("setauthcode")]
        public IActionResult SetAuthCode([FromQuery]string code)
        {
            _iTdClientService.SetAuthCode(code);
            return Ok("Code Set");
        }

        [HttpGet]
        [Route("getchats")]
        public IActionResult GetChats()
        {
            _iTdClientService.GetChats();
            return Ok("Executing Getting Chats");
        }
        
        [HttpGet]
        [Route("logout")]
        public IActionResult Logout()
        {
            _iTdClientService.Logout();
            return Ok("Logged Out");
        }

        #endregion
    }
}