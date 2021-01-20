using FunAtWorkplace.Application.DTO;
using FunAtWorkplace.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FunAtWorkplace.Service.Controllers
{
    [Route("api/messages")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private ILogger<MessagesController> _logger;
        private IMessageService _messageService;

        public MessagesController(IMessageService messageService, ILogger<MessagesController> logger)
        {
            _messageService = messageService ?? throw new ArgumentNullException(nameof(messageService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public IActionResult Post([FromBody] MessageRequest messageRequest)
        {
            try
            {
                var status = _messageService.SendMessage(messageRequest);
                if (status)
                    return Ok();
                return BadRequest("Could not send message");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ex);
            }
        }
    }
}
