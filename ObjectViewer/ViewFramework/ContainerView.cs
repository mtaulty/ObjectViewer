using Autofac;
using ObjectViewer.BindingFramework;
using ObjectViewer.Extensions;
using ObjectViewer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ObjectViewer.ViewFramework
{
    internal abstract class ContainerView : View, IViewModelLocator
    {
        public ContainerView(IComponentContext componentContext, IViewModelLocator viewModelLocator) : base(componentContext, viewModelLocator)
        {
        }
        public override void InitialiseViewModelAndBind(IViewModelLocator viewModelLocator = null)
        {
            // We bind any properties that we have which are public and of type Notifiable<T>
            base.InitialiseViewModelAndBind();

            // We want to find all the child properties we have that are of type View.
            this.ChildViewProperties.ForAll(
                property =>
                {
                    // Instantiate it and set the value in the property.
                    var childView = (View)base.ComponentContext.Resolve(property.PropertyType);
                    property.SetValue(this, childView);

                    childView.AddPreferredViewModelName(property.Name);
                    
                    var viewBindingName = BindingFactory.GetBindingName<BindsAsAttribute>(property);

                    if (viewBindingName != property.Name)
                    {
                        childView.AddPreferredViewModelName(viewBindingName);
                    }
                    childView.InitialiseViewModelAndBind(this);
                }
            );
        }
        protected abstract void BeginDraw();
        protected abstract void EndDraw();
        public override void Draw()
        {
            this.BeginDraw();

            this.ForAllChildViews(view => view.Draw());

            this.EndDraw();
        }
        protected void ForAllChildViews(Action<View> action)
        {
            this.ChildViewProperties.ForAll(p => action(p.GetValue(this) as View));
        }
        public bool HasViewModel(string viewModelName)
        {
            // Do we have a property with a "BindsTo" attribute of this name?
            // Do we have a property with this name?
            // Does our view model locator have a view model with this name?
            throw new NotImplementedException();
        }
        public object GetViewModelInstance(string viewModelName)
        {
            throw new NotImplementedException();
        }
        protected IEnumerable<PropertyInfo> ChildViewProperties => 
            this.GetType().GetProperties().Where(p => p.PropertyType.IsSubclassOf(typeof(View)));
    }
}