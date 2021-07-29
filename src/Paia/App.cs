using Paia.Views;
using System;

namespace Paia
{
    public class App
    {
        private readonly IServiceProvider serviceProvider;
        private readonly ViewManager viewManager;

        public App(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            viewManager = new ViewManager(serviceProvider);
        }

        public int Run<TStartUpView>() where TStartUpView : IView
        {
            viewManager.AddView<TStartUpView>();

            return RunApp();
        }

        public int Run<TStartUpView>(Action<TStartUpView> context) where TStartUpView : IView
        {
            viewManager.AddView<TStartUpView>();

            IView view = viewManager.GetCurrentViewInstance();
            context?.Invoke((TStartUpView)view);

            return RunApp();
        }

        private int RunApp()
        {
            IView view;
            ViewResult result = null;
            
            do
            {
                view = viewManager.GetCurrentViewInstance();

                result?.ViewContext?.Invoke(view);
                result = view.Render();

                HandleViewAction(result);
            } while (result.Action != ViewAction.Exit);

            return result.ExitCode;
        }

        private void HandleViewAction(ViewResult result)
        {
            switch (result.Action)
            {
                case ViewAction.ChangeView:
                    viewManager.AddView(result.ViewType);
                    break;

                case ViewAction.RefreshView:
                    viewManager.DiscardCurrentViewInstance();
                    break;

                case ViewAction.GoBack:
                    viewManager.ChangeToPreviousView();
                    break;

                case ViewAction.GoForward:
                    viewManager.ChangeToNextView();
                    break;

                case ViewAction.NoAction:
                case ViewAction.Exit:
                default:
                    break;
            }
        }
    }
}
