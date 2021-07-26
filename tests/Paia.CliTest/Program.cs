using Paia.Views;
using Paia.Services;
using Paia.CliTest.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Paia
{
    class Program : ViewBase
    {
        static int Main(string[] args)
        {
            return new AppBuilder()
                .ConfigureServiceCollection(ConfigureServices)
                .Build()
                .Run<HomeView>(context => context.Header = "Hello World! :)");
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IFakeAuthManager>(new FakeAuthManager() { UserName = "Abooow" });
            services.AddSingleton<IPrintHello, PrintHello>();
        }
    }
}
