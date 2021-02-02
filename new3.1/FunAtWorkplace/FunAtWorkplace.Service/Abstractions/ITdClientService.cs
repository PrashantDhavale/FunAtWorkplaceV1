namespace FunAtWorkplace.Service.Abstractions
{
    public interface ITdClientService
    {
        void SetAuthCode(string code);
        void GetChats();
        void Logout();
        void SendMessage(string message);
    }

    public interface ITdClient
    {
        void SetAuthCode(string code);
        void GetChats();
        void Logout();
        void SendMessage(string message);
    }
}