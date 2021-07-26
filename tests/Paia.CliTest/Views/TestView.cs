using Paia.Attributes;
using Paia.Components;
using Paia.Services;
using System;
using System.Collections.Generic;

namespace Paia.Views
{
    class TestView : ViewBase
    {
        [Inject]
        public IFakeAuthManager AuthManager { get; set; }

        public IEnumerable<string> Data { get; set; }

        public override ViewResult Render()
        {
            Console.Clear();

            ViewManager.RenderComponent<NewAuthStateComponent>();
            foreach (var item in Data)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine();

            Console.WriteLine(AuthManager.UserName + " select the following..");
            Console.WriteLine("Press 1 to go back or 2 to exit");
            char input = Console.ReadKey().KeyChar;

            return input switch
            {
                '1' => GoBack(),
                '2' => Exit(),
                _   => ReRenderView()
            };
        }
    }
}
