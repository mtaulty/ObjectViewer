using ObjectViewer.BindingFramework.Attributes;
using ObjectViewer.ViewFramework;
using ObjectViewer.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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
        static IEnumerable<PropertyInfo> NotifiablePropertiesMatchingHierarchyPath(Type type, string hierarchyPath)
        {
            return (type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(
                    p => p.PropertyType.IsGenericType &&
                    (p.GetCustomAttribute(typeof(BindingHiddenAttribute)) == null) &&
                    (p.PropertyType.GetGenericTypeDefinition() == typeof(Notifiable<>)) &&
                    (GetBindsToName(p).StartsWith(hierarchyPath))));
        }
        static string GetBindingName<T>(PropertyInfo p) where T : BindsAttribute
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

        public static IEnumerable<IBinding> CreateViewViewModelBindings(
            View view, string hierarchyPath, object viewModel)
        {
            var viewProperties = NotifiableProperties(view.GetType()).Select(
                p => new
                {
                    Name = hierarchyPath + GetBindsAsName(p),
                    p.PropertyType,
                    Value = p.GetValue(view),
                    GenericArgumentTypes = new[] { p.PropertyType.GetGenericArguments().First() }
                }
            );
            var viewModelProperties = NotifiablePropertiesMatchingHierarchyPath(viewModel.GetType(), hierarchyPath).Select(
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