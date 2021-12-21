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
        static string GetBindingName(PropertyInfo p) =>
            p.GetCustomAttribute<BindingPropertyNameAttribute>()?.Name ?? p.Name;

        static string GetBindingSourceName(PropertyInfo p) =>
            p.GetCustomAttribute<BindingSourcePropertyAttribute>()?.Name ?? p.Name;

        public static IEnumerable<IBinding> CreateViewViewModelBindings(
            View view, object viewModel)
        {
            var viewProperties = NotifiableProperties(view.GetType()).Select(
                p => new
                {
                    Name = GetBindingName(p),
                    p.PropertyType,
                    Value = p.GetValue(view),
                    GenericArgumentTypes = new[] { p.PropertyType.GetGenericArguments().First() }
                }
            );
            var viewModelProperties = NotifiableProperties(viewModel.GetType()).Select(
                p => new
                {
                    Name = GetBindingSourceName(p),
                    p.PropertyType,
                    Value = p.GetValue(viewModel),
                }
            );
            foreach (var viewProperty in viewProperties)
            {
                var viewModelProperty =
                    viewModelProperties.FirstOrDefault
                        (vm => (vm.Name == viewProperty.Name) && (vm.PropertyType == viewProperty.PropertyType));

                if (viewModelProperty != null)
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