using ObjectViewer.BindingFramework.Attributes;
using ObjectViewer.ViewFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Windows.Devices.Enumeration.Pnp;

namespace ObjectViewer.BindingFramework
{
    internal static class BindingFactory
    {
        static IEnumerable<PropertyInfo> NotifiableProperties(Type type)
        {
            return (type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(
                    p => p.PropertyType.IsGenericType &&
                    (p.GetCustomAttribute(typeof(BindingHiddenAttribute)) == null) &&
                    (p.PropertyType.GetGenericTypeDefinition() == typeof(Notifiable<>))));
        }
        public static string GetBindingName<T>(PropertyInfo p) where T : BindsAttribute
        {
            var propertyName = p.Name;
            var bindsAsAttribute = p.GetCustomAttribute<T>();
         
            if (bindsAsAttribute != null)
            {
                propertyName = bindsAsAttribute.Name;
            }
            return (propertyName);
        }
        static string GetBindsToName(PropertyInfo p) => GetBindingName<BindsToAttribute>(p);
        static string GetBindsAsName(PropertyInfo p) => GetBindingName<BindsAsAttribute>(p);

        public static IEnumerable<IBinding> CreateViewViewModelBindings(View view, object viewModel)
        {
            var viewProperties = NotifiableProperties(view.GetType()).Select(
                p => new
                {
                    Name = GetBindsAsName(p),
                    p.PropertyType,
                    Value = p.GetValue(view),
                    GenericArgumentTypes = new[] { p.PropertyType.GetGenericArguments().First() }
                }
            );
            var viewModelProperties = NotifiableProperties(viewModel.GetType()).Select(
                p => new
                {
                    Name = GetBindsToName(p),
                    p.PropertyType,
                    Value = p.GetValue(viewModel),
                }
            );
            foreach (var viewModelProperty in viewModelProperties)
            {
                var viewProperty =
                    viewProperties.FirstOrDefault
                        (vp => (vp.Name == viewModelProperty.Name) && (vp.PropertyType == viewModelProperty.PropertyType));

                if (viewProperty != null)
                {
                    var genericType = typeof(Binding<>).MakeGenericType(viewProperty.GenericArgumentTypes);

                    var binding = Activator.CreateInstance(genericType, viewProperty.Value, viewModelProperty.Value) as IBinding;

                    binding.AddHandlers();

                    yield return binding;
                }
            }
        }
    }
}