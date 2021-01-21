namespace FunAtWorkplace.Service.Abstractions
{
    public interface ITdClient
    {
        void InitializeTdClient();
        void SetAuthCodeTdClient(string code);
    }
}
