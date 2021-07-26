using System;
using Paia.Components;

namespace Paia.Views
{
    internal sealed class HomeView : ViewBase
    {
        public string Header { get; set; }

        public override ViewResult Render()
        {
            Console.Clear();

            ViewManager.RenderComponent<AuthStateComponent>();
            Console.WriteLine(Header ?? "Hello World! :)");

            Console.WriteLine();
            Console.WriteLine("Press a key to continue...");
            Console.ReadKey();

            return ChangeView<TestView>(context => context.Data = new string[] { "Line 1", "Hello", "Line 3" });
        }
    }
}
