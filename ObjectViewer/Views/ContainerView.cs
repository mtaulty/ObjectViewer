using Autofac;
using ObjectViewer.Extensions;
using System;
using System.Collections.Generic;

namespace ObjectViewer.Views
{
    internal abstract class ContainerView : View
    {
        protected Dictionary<string, View> Children { get; set; }
        public ContainerView(IComponentContext componentContext) : base(componentContext)
        {
        }
        protected virtual void AddChildView(string viewName, View view)
        {
            if (this.Children == null)
            {
                this.Children = new Dictionary<string,View>();
            }
            if (this.Bindings.Count > 0)
            {
                throw new InvalidOperationException("Cannot add children post binding (right now)");
            }
            if (this.Children.ContainsKey(viewName))
            {
                throw new ArgumentException($"Already have a child view named $viewName");
            }
            this.Children[viewName] = view;
        }
        public override void Initialise()
        {
            this.Children.ForAll(child => child.Value.Initialise());
        }
        protected abstract void BeginDraw();
        protected abstract void EndDraw();
        public override void Draw()
        {
            this.BeginDraw();

            this.Children.ForAll(
                child =>
                {
                    child.Value.Draw();
                }
            );
            this.EndDraw();
        }
    }
}