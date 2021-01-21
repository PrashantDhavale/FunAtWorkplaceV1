using FunAtWorkplace.Service.Abstractions;
using System;
using System.Threading;
using Td = Telegram.Td;
using TdApi = Telegram.Td.Api;

namespace FunAtWorkplace.Service.Infrastructure.Repositories
{
    public class TdClient : ITdClient
    {
        #region Members

        private static Td.Client _client = null;
        private readonly static Td.ClientResultHandler _defaultHandler = new DefaultHandler();

        private static TdApi.AuthorizationState _authorizationState = null;
        private static volatile bool _haveAuthorization = false;
        private static volatile bool _needQuit = false;

        private static volatile AutoResetEvent _gotAuthorization = new AutoResetEvent(false);

        private static readonly string _newLine = Environment.NewLine;
        private static volatile string _currentPrompt = null;

        #endregion

        #region Methods
        
        public void InitializeTdClient()
        {
            Td.Client.Execute(new TdApi.SetLogVerbosityLevel(0));
            _client = CreateTdClient();
            // await authorization
            _gotAuthorization.Reset();
            _gotAuthorization.WaitOne();

            _client.Send(new TdApi.GetChats(null, Int64.MaxValue, 0, 100), _defaultHandler); // preload main chat list
        }

        public void SetAuthCodeTdClient(string code)
        {
            if (_authorizationState is TdApi.AuthorizationStateWaitCode)
            {
                _client.Send(new TdApi.CheckAuthenticationCode(code), new AuthorizationRequestHandler());
            }
        }

        private static Td.Client CreateTdClient()
        {
            Console.WriteLine("AuthStatus in create" + _authorizationState);
            Td.Client result = Td.Client.Create(new UpdateHandler());
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                result.Run();
            }).Start();
            return result;
        }

        private static void OnAuthorizationStateUpdated(TdApi.AuthorizationState authorizationState)
        {
            Console.WriteLine("AuthStatus: " + _authorizationState);

            if (authorizationState != null)
            {
                _authorizationState = authorizationState;
            }
            if (_authorizationState is TdApi.AuthorizationStateWaitTdlibParameters)
            {
                TdApi.TdlibParameters parameters = new TdApi.TdlibParameters();
                parameters.DatabaseDirectory = "tdlib";
                parameters.UseMessageDatabase = true;
                parameters.UseSecretChats = true;
                parameters.ApiId = 2179654;
                parameters.ApiHash = "c5252b873118c679a8aff474080987c4";
                parameters.SystemLanguageCode = "en";
                parameters.DeviceModel = "Desktop";
                parameters.ApplicationVersion = "1.0";
                parameters.EnableStorageOptimizer = true;

                _client.Send(new TdApi.SetTdlibParameters(parameters), new AuthorizationRequestHandler());
            }
            else if (_authorizationState is TdApi.AuthorizationStateWaitEncryptionKey)
            {
                _client.Send(new TdApi.CheckDatabaseEncryptionKey(), new AuthorizationRequestHandler());
            }
            else if (_authorizationState is TdApi.AuthorizationStateWaitPhoneNumber)
            {
                string phoneNumber = "+610478963392";
                _client.Send(new TdApi.SetAuthenticationPhoneNumber(phoneNumber, null), new AuthorizationRequestHandler());
            }
            //else if (_authorizationState is TdApi.AuthorizationStateWaitCode)
            //{
            //    string code = ReadLine("Please enter authentication code: ");
            //    _client.Send(new TdApi.CheckAuthenticationCode(code), new AuthorizationRequestHandler());
            //}
            else if (_authorizationState is TdApi.AuthorizationStateReady)
            {
                _haveAuthorization = true;
                _gotAuthorization.Set();
            }
            //else if (_authorizationState is TdApi.AuthorizationStateLoggingOut)
            //{
            //    _haveAuthorization = false;
            //    Print("Logging out");
            //}
            //else if (_authorizationState is TdApi.AuthorizationStateClosing)
            //{
            //    _haveAuthorization = false;
            //    Print("Closing");
            //}
            //else
            //{
            //    Print("Unsupported authorization state:" + _newLine + _authorizationState);
            //}
        }

        private static void Print(string str)
        {
            if (_currentPrompt != null)
            {
                Console.WriteLine();
            }
            Console.WriteLine(str);
            if (_currentPrompt != null)
            {
                Console.Write(_currentPrompt);
            }
        }

        private static string ReadLine(string str)
        {
            Console.Write(str);
            _currentPrompt = str;
            var result = Console.ReadLine();
            _currentPrompt = null;
            return result;
        }

        private static long GetChatId(string arg)
        {
            long chatId = 0;
            try
            {
                chatId = Convert.ToInt64(arg);
            }
            catch (FormatException)
            {
            }
            catch (OverflowException)
            {
            }
            return chatId;
        }

        private static void SendMessage(long chatId, string message)
        {
            TdApi.InlineKeyboardButton[] row = { new TdApi.InlineKeyboardButton("https://telegram.org?1", new TdApi.InlineKeyboardButtonTypeUrl()), new TdApi.InlineKeyboardButton("https://telegram.org?2", new TdApi.InlineKeyboardButtonTypeUrl()), new TdApi.InlineKeyboardButton("https://telegram.org?3", new TdApi.InlineKeyboardButtonTypeUrl()) };
            TdApi.ReplyMarkup replyMarkup = new TdApi.ReplyMarkupInlineKeyboard(new TdApi.InlineKeyboardButton[][] { row, row, row });

            TdApi.InputMessageContent content = new TdApi.InputMessageText(new TdApi.FormattedText(message, null), false, true);
            _client.Send(new TdApi.SendMessage(chatId, 0, 0, null, replyMarkup, content), _defaultHandler);
        }

        #endregion

        #region Handlers

        private class DefaultHandler : Td.ClientResultHandler
        {
            void Td.ClientResultHandler.OnResult(TdApi.BaseObject @object)
            {
                Print(@object.ToString());
            }
        }

        private class UpdateHandler : Td.ClientResultHandler
        {
            void Td.ClientResultHandler.OnResult(TdApi.BaseObject @object)
            {
                if (@object is TdApi.UpdateAuthorizationState)
                {
                    OnAuthorizationStateUpdated((@object as TdApi.UpdateAuthorizationState).AuthorizationState);
                }
            }
        }

        private class AuthorizationRequestHandler : Td.ClientResultHandler
        {
            void Td.ClientResultHandler.OnResult(TdApi.BaseObject @object)
            {
                if (@object is TdApi.Error)
                {
                    Print("Receive an error:" + _newLine + @object);
                    OnAuthorizationStateUpdated(null); // repeat last action
                }
            }
        }

        #endregion
    }
}