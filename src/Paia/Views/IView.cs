using Paia.Components;

namespace Paia.Views
{
    public interface IView : IComponent
    {
        new ViewResult Render();
    }
}
