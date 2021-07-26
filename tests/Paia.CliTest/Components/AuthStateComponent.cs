using Paia.Attributes;
using Paia.Services;
using System;

namespace Paia.Components
{
    class AuthStateComponent : ComponentBase
    {
        [Inject]
        public IFakeAuthManager AuthManager { get; set; }

        public string Wrapper { get; set; }

        public AuthStateComponent()
        {
            Wrapper = "~~~~~~~~~~~~~~~~~~~~~~~~~~";
        }

        public override void Render()
        {
            DrawWrapper();

            if (AuthManager.IsAuthorized)
                Console.WriteLine($"Hello, {AuthManager.UserName}!");
            else
                Console.WriteLine("Not authorized.");
            
            DrawWrapper();
        }

        protected void DrawWrapper()
        {
            if (Wrapper is not null)
                Console.WriteLine(Wrapper); 
        }
    }
}
