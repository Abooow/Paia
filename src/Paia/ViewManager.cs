using Paia.Views;
using Paia.Components;
using Paia.Services;
using System;

namespace Paia
{
    public class ViewManager
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IInjectServices injectServices;
        private readonly NavigationStack<View> navigationStack;

        private View currentView => navigationStack.Current;

        public ViewManager(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            injectServices = new PropertyServiceInjector();
            navigationStack = new NavigationStack<View>();
        }

        public void AddView(Type viewType)
        {
            navigationStack.Push(new View(viewType));
        }

        public void AddView<TView>() where TView : IView
        {
            AddView(typeof(TView));
        }

        public void DiscardCurrentViewInstance()
        {
            currentView.Instance = null;
        }

        public void ChangeToPreviousView()
        {
            navigationStack.Back();
        }

        public void ChangeToNextView()
        {
            navigationStack.Forward();
        }

        public void RenderComponent<TComponent>() where TComponent : IComponent
        {
            IComponent component = GetComponent<TComponent>();
            component.Render();
        }

        public void RenderComponent<TComponent>(Action<TComponent> context) where TComponent : IComponent
        {
            IComponent component = GetComponent<TComponent>();
            context?.Invoke((TComponent)component);
            component.Render();
        }

        public IView GetCurrentViewInstance()
        {
            IView view = currentView.Instance is not null
                ? currentView.Instance
                : GetView(currentView.Type);

            currentView.Instance = view;
            view.ViewManager = this;

            return view;
        }

        private IView GetView(Type viewType)
        {
            IView view = GetComponent(viewType) as IView;

            if (view is null)
                throw new Exception($"Invalid ViewType {currentView.Type}. The view must inherit from IView");

            return view;
        }

        private IComponent GetComponent<TComponent>() where TComponent : IComponent
        {
            return GetComponent(typeof(TComponent));
        }

        private IComponent GetComponent(Type componentType)
        {
            IComponent component = injectServices.InjectServices<IComponent>(serviceProvider, componentType);
            component.ViewManager = this;
            component.OnInitialized();

            return component;
        }

        private class View
        {
            public Type Type { get; }
            public IView Instance { get; set; }

            public View(Type type)
            {
                Type = type;
                Instance = null;
            }
        }
    }
}
