namespace Paia.Services
{
    internal class FakeAuthManager : IFakeAuthManager
    {
        public string UserName { get; set; }
        public bool IsAuthorized => UserName is not null;
    }
}
