using Paia.Attributes;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Paia.Services
{
    /// <summary>
    /// Injects services into properties to a given Type whos properties that are labeled with an [Inject] attribute.
    /// </summary>
    internal sealed class PropertyServiceInjector : IInjectServices
    {
        private readonly Dictionary<Type, Action<IServiceProvider, object>> cachedInitializers;

        public PropertyServiceInjector()
        {
            cachedInitializers = new();
        }

        public TInstance InjectServices<TInstance>(IServiceProvider serviceProvider, Type toType)
        {
            var component = Activator.CreateInstance(toType);
            if (component is null)
                throw new InvalidOperationException($"The component activator returned a null value for a component of type {toType.FullName}.");

            PerformPropertyInjection(serviceProvider, component);

            return (TInstance)component;
        }

        public void ClearCache()
        {
            cachedInitializers.Clear();
        }

        private void PerformPropertyInjection(IServiceProvider serviceProvider, object instance)
        {
            Type instanceType = instance.GetType();
            if (!cachedInitializers.TryGetValue(instanceType, out var initializer))
            {
                initializer = CreateInitializer(instanceType);
                cachedInitializers.TryAdd(instanceType, initializer);
            }

            initializer(serviceProvider, instance);
        }

        private Action<IServiceProvider, object> CreateInitializer(Type type)
        {
            // Do all the reflection up front
            List<(string name, Type propertyType, PropertyInfo propertyInfo)>? injectables = null;
            foreach (var property in GetInjectPropertiesIncludingInherited(type))
            {
                injectables ??= new();
                injectables.Add((property.Name, property.PropertyType, property));
            }

            if (injectables is null)
                return static (_, _) => { };

            return Initialize;

            void Initialize(IServiceProvider serviceProvider, object component)
            {
                foreach (var (propertyName, propertyType, propertyInfo) in injectables)
                {
                    var serviceInstance = serviceProvider.GetService(propertyType);
                    if (serviceInstance == null)
                    {
                        throw new InvalidOperationException($"Cannot provide a value for property " +
                            $"'{propertyName}' on type '{type.FullName}'. There is no " +
                            $"registered service of type '{propertyType}'.");
                    }

                    propertyInfo.SetValue(component, serviceInstance);
                }
            }
        }

        private IEnumerable<PropertyInfo> GetInjectPropertiesIncludingInherited(Type type)
        {
            Type currentType = type;

            while (currentType.BaseType != null)
            {
                PropertyInfo[] properties = currentType.GetProperties(BindingFlags.Public | BindingFlags.Instance 
                                                                     | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);

                foreach (PropertyInfo property in properties)
                {
                    if (property.IsDefined(typeof(InjectAttribute)))
                        yield return property;
                }

                currentType = currentType.BaseType;
            }
        }
    }
}
