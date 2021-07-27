using Paia.Attributes;
using Paia.Views;
using TutorialSample.Services;
using System;

namespace TutorialSample.Views
{
    internal sealed class MyFirstView : ViewBase
    {
        [Inject]
        public IRandomSentenceGenerator RandomSentenceGenerator { get; set; }

        private string cachedSentence;

        public override ViewResult Render()
        {
            Console.Clear();

            // This shows that ReRenderView() does not create a new instance.
            cachedSentence ??= RandomSentenceGenerator.GetRandomSentence();

            Console.WriteLine("Hello World!");
            Console.WriteLine("Cached sentence: " + cachedSentence);
            Console.WriteLine("Not cached sentence: " + RandomSentenceGenerator.GetRandomSentence());

            Console.WriteLine();
            Console.WriteLine("Press 1 to change View, 2 to exit");
            char input = Console.ReadKey().KeyChar;

            return input switch
            {
                '1' => ChangeView<MySecondView>(context => context.Message = "Hello :)"),
                '2' => Exit(),
                 _ => ReRenderView()
            };
        }
    }
}
