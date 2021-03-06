using TutorialSample.Services;
using TutorialSample.Views;
using Paia;
using Microsoft.Extensions.DependencyInjection;

namespace TutorialSample
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            new AppBuilder()
                .ConfigureServices(services => services.AddSingleton<IRandomSentenceGenerator, RandomSentenceGenerator>())
                .Build()
                .Run<MyFirstView>(context => context.Message = "Yo World!");
        }
    }
}
