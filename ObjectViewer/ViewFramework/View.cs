using Autofac;
using ObjectViewer.BindingFramework;
using ObjectViewer.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ObjectViewer.ViewFramework
{
    internal abstract class View
    {
        public View(IComponentContext componentContext, IViewModelLocator viewModelLocator)
        {
            this.ComponentContext = componentContext;
            this.ViewModelLocator = viewModelLocator;
            this.Bindings = new List<IBinding>();
            this.possibleViewModelNames = new List<string>();
            this.possibleViewModelNames.Add(this.GetType().Name + "Model");
        }
        protected IViewModelLocator ViewModelLocator { get; private set; }
        protected IComponentContext ComponentContext { get; private set; }

        public void AddPreferredViewModelName(string viewModelName)
        {
            this.possibleViewModelNames.Insert(0, viewModelName);
        }
        public virtual void InitialiseViewModelAndBind(IViewModelLocator overrideLocator = null)
        {
            var locator = overrideLocator ?? this.ViewModelLocator;

            foreach (var viewModelName in this.possibleViewModelNames)
            {
                if (locator.HasViewModel(viewModelName))
                {
                    this.viewModel = locator.GetViewModelInstance(viewModelName);
                    this.CreateBindings();
                    break;
                }
            }
        }
        public virtual void InitialiseResources()
        {
        }
        public abstract void Draw();

        protected virtual void OnViewModelChanged(object sender, ChangeNotificationEventArgs<object> e)
        {
            if (e.OldValue != null)
            {
                this.ClearBindings();
            }
            this.CreateBindings();
        }
        protected virtual void CreateBindings()
        {
            if (this.viewModel != null)
            {
                foreach (var binding in BindingFactory.CreateViewViewModelBindings(this, this.viewModel))
                {
                    this.Bindings.Add(binding);
                    binding.ViewModelToView();
                }
            }
        }
        protected virtual void ClearBindings()
        {
            foreach (var binding in this.Bindings)
            {
                binding.RemoveHandlers();
            }
            this.Bindings.Clear();
        }
        protected bool ViewModelHasPropertyNamed(string propertyName)
        {
            return (this.viewModel?.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance) != null);
        }
        protected bool ViewModelHasPropertyWhichBindsTo(string propertyName)
        {
            return (this.GetViewModelPropertyWhichBindsTo(propertyName) != null);
        }
        protected PropertyInfo GetViewModelPropertyWhichBindsTo(string propertyName)
        {
            PropertyInfo propertyInfo = null;

            var properties = this.GetViewModelPublicProperties();
            if (properties != null)
            {
                propertyInfo = properties.FirstOrDefault(p => BindingFactory.GetBindingName<BindsToAttribute>(p) == propertyName);
            }
            return (propertyInfo);
        }
        protected object GetViewModelPropertyValue(string propertyName)
        {
            return (this.viewModel.GetType().GetProperty(propertyName).GetValue(this.viewModel));
        }

        PropertyInfo[] GetViewModelPublicProperties() => this.viewModel?.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

        List<IBinding> Bindings { get; set; }
        List<string> possibleViewModelNames;
        object viewModel;
    }
}