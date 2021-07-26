using Paia.Attributes;
using Paia.CliTest.Services;
using System;

namespace Paia.Components
{
    class NewAuthStateComponent : AuthStateComponent
    {
        [Inject]
        public IPrintHello PrintHello { get; set; }

        public override void Render()
        {
            PrintHello.Print();
            Console.WriteLine(" from Auth2!");

            Console.WriteLine("Welcome, " + AuthManager.UserName ?? "<Anonymous>");
            DrawWrapper();
        }
    }
}
