using Microsoft.Extensions.DependencyInjection;
using System;

namespace Paia
{
    public class AppBuilder
    {
        private readonly IServiceCollection serviceCollection;
        private bool appBuilt;

        public AppBuilder()
        {
            serviceCollection = new ServiceCollection();
        }

        public AppBuilder ConfigureServices(Action<IServiceCollection> context)
        {
            context?.Invoke(serviceCollection);
            return this;
        }

        public App Build()
        {
            if (appBuilt)
                throw new Exception("Build can only be called once.");

            appBuilt = true;
            return new App(serviceCollection.BuildServiceProvider());
        }
    }
}
