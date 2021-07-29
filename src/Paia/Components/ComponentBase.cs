using System;

namespace Paia.Components
{
    public abstract class ComponentBase : IComponent
    {
        public ViewManager ViewManager { get; set; }

        // Don't force subclasses to override this method.
        public virtual void OnInitialized() { }

        public abstract void Render();

        protected void RenderComponent<TComponent>() where TComponent : IComponent
        {
            ViewManager.RenderComponent<TComponent>();
        }

        protected void RenderComponent<TComponent>(Action<TComponent> context) where TComponent : IComponent
        {
            ViewManager.RenderComponent<TComponent>(context);
        }
    }
}
