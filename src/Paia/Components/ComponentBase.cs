namespace Paia.Components
{
    public abstract class ComponentBase : IComponent
    {
        public ViewManager ViewManager { get; set; }

        public abstract void Render();
    }
}
