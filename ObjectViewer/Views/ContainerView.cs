using Autofac;
using ObjectViewer.BindingFramework;
using ObjectViewer.Extensions;
using System;
using System.Collections.Generic;

namespace ObjectViewer.Views
{
    internal abstract class ContainerView : View
    {
        protected List<View> Children { get; set; }

        public ContainerView(IComponentContext componentContext) : base(componentContext)
        {
        }
        protected virtual void AddChildView(string viewName, View view)
        {
            if (this.Children == null)
            {
                this.Children = new List<View>();
            }
            if (this.Bindings.Count > 0)
            {
                throw new InvalidOperationException("Cannot add children post binding (right now)");
            }
            View.AddToViewHierarchyPath(view, viewName);
            this.Children.Add(view);
        }
        public override void Initialise()
        {
            this.Children.ForAll(child => child.Initialise());
        }
        protected abstract void BeginDraw();
        protected abstract void EndDraw();
        public override void Draw()
        {
            this.BeginDraw();

            this.Children.ForAll(
                child =>
                {
                    child.Draw();
                }
            );
            this.EndDraw();
        }
        protected override void CreateBindings()
        {
            // Create all the bindings that are straight from the properties of this view
            // to some viewmodel.
            base.CreateBindings();

            // Now look at the children of this view and see if we can bind them up.
            this.Children.ForAll(
                child => 
                {
                    foreach (var binding in BindingFactory.CreateViewViewModelBindings(child, View.GetViewHierarchyPath(child), this.ViewModel.Value))
                    {
                        this.Bindings.Add(binding);
                        binding.ViewModelToView();
                    }
                }
            );
        }
    }
}