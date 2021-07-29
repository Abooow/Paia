using System;

namespace Paia.Services
{
    internal interface IInjectServices
    {
        TInstance InjectServices<TInstance>(IServiceProvider serviceProvider, Type toType);
        void ClearCache();
    }
}
