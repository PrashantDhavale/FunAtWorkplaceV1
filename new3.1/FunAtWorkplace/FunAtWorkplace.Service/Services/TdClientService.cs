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

        public void SendMessage(string message)
        {
            _tdClient.SendMessage(message);
        }

        public void SetAuthCode(string code)
        {
            _tdClient.SetAuthCode(code);
        }

        public void Logout()
        {
            _tdClient.Logout();
        }

        public void GetChats()
        {
            _tdClient.GetChats();
        }

        #endregion
    }
}