using FunAtWorkplace.Service.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace FunAtWorkplace.Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TdController : ControllerBase
    {
        #region Members

        private readonly ILogger<TdController> _logger;
        private readonly ITdClientService _tdClientService;


        #endregion

        #region Constructor

        public TdController(ITdClientService tdClientService,
                            ILogger<TdController> logger)
        {
            _tdClientService = tdClientService ?? throw new ArgumentNullException(nameof(tdClientService));
            _logger = logger;
        }

        #endregion

        #region Methods

        [HttpGet]
        [Route("initialize")]
        public IActionResult Initialize()
        {
            _tdClientService.InitializeTdClient();
            return Ok("Td Client Initialized");
        }

        [HttpGet]
        [Route("setcode")]
        public IActionResult SetAuthCode([FromQuery] string code)
        {
            _tdClientService.SetAuthCodeTdClient(code);
            return Ok("Td Client Code Passed in");
        }

        #endregion
    }
}