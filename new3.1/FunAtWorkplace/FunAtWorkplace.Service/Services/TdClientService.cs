using FunAtWorkplace.Service.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FunAtWorkplace.Service.Services
{
    public class TdClientService : ITdClientService
    {
        #region Members

        private readonly ITdClient _tdClient;
        private readonly ILogger<TdClientService> _logger;

        #endregion

        #region Constructor

        public TdClientService(ITdClient tdClient,
                               ILogger<TdClientService> logger)
        {
            _tdClient = tdClient ?? throw new ArgumentNullException(nameof(tdClient));
            _logger = logger;
        }

        #endregion

        #region Methods

        public bool InitializeTdClient()
        {
            _tdClient.InitializeTdClient();
            Task.Delay(5000);
            return true;
        }

        public void SetAuthCodeTdClient(string code)
        {
            _tdClient.SetAuthCodeTdClient(code);
            Task.Delay(5000);
        }

        #endregion
    }
}
