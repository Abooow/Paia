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

        public virtual ViewResult Render() 
        {
            return ViewResult.Exit();
        }

        protected static ViewResult ReRenderView()
        {
            return new ViewResult(ViewAction.None);
        }

        protected static ViewResult ChangeView<TView>() where TView : IView
        {
            return ViewResult.NewView<TView>();
        }

        protected static ViewResult ChangeView<TView>(Action<TView> context) where TView : IView
        {
            ViewResult viewResult = ViewResult.NewView<TView>();
            viewResult.ViewContext = (x) => context.Invoke((TView)x);

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
