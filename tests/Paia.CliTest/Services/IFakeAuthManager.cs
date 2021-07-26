namespace Paia.Services
{
    interface IFakeAuthManager
    {
        string UserName { get; set; }
        bool IsAuthorized { get; }
    }
}
