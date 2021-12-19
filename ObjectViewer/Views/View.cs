using Autofac;
using ObjectViewer.BindingFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ObjectViewer.Views
{
    class View : IDrawable
    {
        [BindingHidden]
        public Notifiable<object> ViewModel { get; }

        public View(IComponentContext componentContext)
        {
            this.bindings = new List<IBinding>();
            this.ViewModel = new Notifiable<object>();
            this.ViewModel.Changed += this.OnViewModelChanged;
            this.ViewModel.SetValue(componentContext.ResolveNamed(this.ViewModelName, typeof(object)));
        }
        protected string ViewModelName => this.GetType().Name + "Model";

        void OnViewModelChanged(object sender, ChangeNotificationEventArgs<object> e)
        {
            if (e.OldValue != null)
            {
                this.ClearBindings();
            }
            if (this.ViewModel.Value != null)
            {
                this.CreateBindings();
            }
        }
        static IEnumerable<PropertyInfo> NotifiableProperties(Type type)
        {
            return (type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(
                    p => p.PropertyType.IsGenericType &&
                    (p.GetCustomAttribute(typeof(BindingHiddenAttribute)) == null) &&
                    (p.PropertyType.GetGenericTypeDefinition() == typeof(Notifiable<>))));
        }
        protected void CreateBindings()
        {
            var viewProperties = NotifiableProperties(this.GetType()).Select(
                p => new
                {
                    p.Name,
                    p.PropertyType,
                    Value = p.GetValue(this),
                    GenericArgumentTypes = new[] { p.PropertyType.GetGenericArguments().First() }
                }
            );
            var viewModelProperties = NotifiableProperties(this.ViewModel.Value.GetType()).Select(
                p => new
                {
                    p.Name,
                    p.PropertyType,
                    Value = p.GetValue(this.ViewModel.Value)
                }
            );
            foreach (var viewProperty in viewProperties)
            {
                var viewModelProperty =
                    viewModelProperties.FirstOrDefault(vm => ((vm.Name == viewProperty.Name) && (vm.PropertyType == viewProperty.PropertyType)));

                if (viewModelProperty != null)
                {
                    var genericType = typeof(Binding<>).MakeGenericType(viewProperty.GenericArgumentTypes);

                    var binding = Activator.CreateInstance(genericType, viewProperty.Value, viewModelProperty.Value) as IBinding;

                    binding.AddHandlers();
                    binding.ViewModelToView();
                    this.bindings.Add(binding);
                }
            }
        }
        void ClearBindings()
        {
            foreach (var binding in this.bindings)
            {
                binding.RemoveHandlers();
            }
            this.bindings.Clear();
        }
        public virtual void Initialise()
        {
        }
        public virtual void Draw()
        {
        }

        List<IBinding> bindings;
    }
}