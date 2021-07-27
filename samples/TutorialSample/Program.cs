using Paia;
using TutorialSample.Services;
using TutorialSample.Views;
using Microsoft.Extensions.DependencyInjection;

namespace TutorialSample
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            new AppBuilder()
                .ConfigureServiceCollection(services => services.AddSingleton<IRandomSentenceGenerator, RandomSentenceGenerator>())
                .Build()
                .Run<MyFirstView>();
        }
    }
}
