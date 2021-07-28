using TutorialSample.Services;
using Paia.Attributes;
using Paia.Views;
using System;

namespace TutorialSample.Views
{
    internal sealed class MyFirstView : ViewBase
    {
        [Inject]
        public IRandomSentenceGenerator RandomSentenceGenerator { get; set; }

        public string Message { get; set; }

        private string cachedSentence;

        public override ViewResult Render()
        {
            Message ??= "Hello World!";

            Console.Clear();

            // This shows that ReRenderView() does not create a new instance.
            cachedSentence ??= RandomSentenceGenerator.GetRandomSentence();

            Console.WriteLine(Message);
            Console.WriteLine("Cached sentence: " + cachedSentence);
            Console.WriteLine("Not cached sentence: " + RandomSentenceGenerator.GetRandomSentence());

            Console.WriteLine();
            Console.WriteLine("Press 1 to change View, 2 to exit");
            char input = Console.ReadKey().KeyChar;

            return input switch
            {
                '1' => ChangeView<MySecondView>(context => context.Name = "Monkey Paia"),
                '2' => Exit(),
                 _ => ReRenderView()
            };
        }
    }
}
