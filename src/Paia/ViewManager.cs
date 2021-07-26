using Paia.Views;
using System;
using Paia.Components;
using Paia.Factories;

namespace Paia
{
    public class ViewManager
    {
        private readonly IServiceProvider serviceProvider;
        private readonly InjectAttrObjectFactory injectObjectFactory;
        private readonly NavigationStack<View> navigationStack;

        private View currentView => navigationStack.Current;

        public ViewManager(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            injectObjectFactory = new InjectAttrObjectFactory();
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

        private IComponent GetComponent<TComponent>() where TComponent : IComponent
        {
            IComponent component = injectObjectFactory.InstantiateComponent(serviceProvider, typeof(TComponent)) as IComponent;
            component.ViewManager = this;

            return component;
        }

        public IView GetCurrentViewInstance()
        {
            IView view = currentView.Instance is not null
                ? currentView.Instance
                : injectObjectFactory.InstantiateComponent(serviceProvider, currentView.Type) as IView;

            if (view is not IView)
                throw new Exception($"Invalid ViewType {currentView.Type}. The view must inherit from IView");

            currentView.Instance = view;
            view.ViewManager = this;

            return view;
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
