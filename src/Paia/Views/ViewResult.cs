using System;

namespace Paia.Views
{
    public sealed class ViewResult
    {
        public Type ViewType { get; init; }
        public Action<IView> ViewContext { get; set; }
        public ViewAction Action { get; init; }
        public int ExitCode { get; init; }

        public ViewResult(ViewAction action)
        {
            Action = action;
        }

        public static ViewResult NoAction()
        {
            return new ViewResult(ViewAction.NoAction);
        }

        public static ViewResult RefreshView()
        {
            return new ViewResult(ViewAction.RefreshView);
        }

        public static ViewResult NewView<TView>() where TView : IView
        {
            return new ViewResult(ViewAction.ChangeView) { ViewType = typeof(TView) };
        }

        public static ViewResult GoBack()
        {
            return new ViewResult(ViewAction.GoBack);
        }

        public static ViewResult GoForward()
        {
            return new ViewResult(ViewAction.GoForward);
        }

        public static ViewResult Exit()
        {
            return new ViewResult(ViewAction.Exit);
        }

        public static ViewResult Exit(int exitCode)
        {
            return new ViewResult(ViewAction.Exit) { ExitCode = exitCode };
        }

        public static implicit operator ViewResult(int exitCode)
        {
            return new ViewResult(ViewAction.Exit) { ExitCode = exitCode };
        }
    }
}
