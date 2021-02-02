using FunAtWorkplace.Service.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace FunAtWorkplace.Service.Controllers
{
    [ApiController()]
    [Route("messages")]
    public class MessagesController : ControllerBase
    {
        #region Members

        private readonly ILogger<MessagesController> _logger;
        private readonly ITdClientService _iTdClientService;

        #endregion

        #region Constructor

        public MessagesController(ITdClientService iTdClientService,
                                  ILogger<MessagesController> logger)
        {
            _iTdClientService = iTdClientService ?? throw new ArgumentNullException(nameof(iTdClientService));
            _logger = logger;
        }

        #endregion

        #region Methods

        [HttpGet]
        [Route("sendmessage")]
        public IActionResult SendMessage([FromQuery]string message)
        {
            _iTdClientService.SendMessage(message);
            return Ok("Message Sent");
        }

        #endregion
    }
}