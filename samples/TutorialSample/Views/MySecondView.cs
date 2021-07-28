using Paia.Views;
using System;

namespace TutorialSample.Views
{
    internal sealed class MySecondView : ViewBase
    {
        public string Name { get; set; }

        public override ViewResult Render()
        {
            Console.Clear();

            Console.WriteLine("Hello World from my Second View!");
            Console.WriteLine($"And hello to you, {Name}!");

            Console.WriteLine();
            Console.WriteLine("Press a key to go back...");
            Console.ReadKey();

            return GoBack();
        }
    }
}
