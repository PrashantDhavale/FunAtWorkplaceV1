namespace FunAtWorkplace.Service.Abstractions
{
    public interface ITdClientService
    {
        bool InitializeTdClient();
        void SetAuthCodeTdClient(string code);
    }
}