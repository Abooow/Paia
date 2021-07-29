using Paia.Components;
using System;

namespace Paia.Views
{
    public abstract class ViewBase : IView
    {
        public ViewManager ViewManager { get; set; }

        void IComponent.Render()
        {
            Render();
        }

        // Don't force subclasses to override this method.
        public virtual void OnInitialized() { }

        public abstract ViewResult Render();

        protected void RenderComponent<TComponent>() where TComponent : IComponent
        {
            ViewManager.RenderComponent<TComponent>();
        }

        protected void RenderComponent<TComponent>(Action<TComponent> context) where TComponent : IComponent
        {
            ViewManager.RenderComponent<TComponent>(context);
        }

        protected static ViewResult ReRenderView()
        {
            return ViewResult.NoAction();
        }

        protected static ViewResult RefreshView()
        {
            return ViewResult.RefreshView();
        }

        protected static ViewResult ChangeView<TView>() where TView : IView
        {
            return ViewResult.NewView<TView>();
        }

        protected static ViewResult ChangeView<TView>(Action<TView> context) where TView : IView
        {
            ViewResult viewResult = ViewResult.NewView<TView>();
            viewResult.ViewContext = x => context.Invoke((TView)x);

            return viewResult;
        }

        protected static ViewResult GoBack()
        {
            return ViewResult.GoBack();
        }

        protected static ViewResult GoForward()
        {
            return ViewResult.GoForward();
        }

        protected static ViewResult Exit()
        {
            return ViewResult.Exit();
        }

        protected static ViewResult Exit(int exitCode)
        {
            return ViewResult.Exit(exitCode);
        }
    }
}
