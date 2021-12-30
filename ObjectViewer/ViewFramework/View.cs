using Autofac;
using ObjectViewer.BindingFramework;
using System.Collections.Generic;

namespace ObjectViewer.ViewFramework
{
    internal abstract class View
    {
        [BindingHidden]
        public Notifiable<object> ViewModel { get; }

        protected string ViewHierarchyPath => this.viewHierarchyPath;
        protected void AddToViewHierarchyPath(string hierarchyPathAddition)
        {
            this.viewHierarchyPath = $"{this.viewHierarchyPath}{hierarchyPathAddition}{hierarchyPathSeparator}";
        }

        protected IComponentContext ComponentContext => this.componentContext;

        protected List<IBinding> Bindings { get; set; }

        public View(IComponentContext componentContext)
        {
            this.viewHierarchyPath = string.Empty;
            this.componentContext = componentContext;
            this.Bindings = new List<IBinding>();
            this.ViewModel = new Notifiable<object>();
            this.ViewModel.Changed += this.OnViewModelChanged;
            this.Initialise();
            this.ViewModel.SetValue(this.LocateViewModel());
        }
        public virtual void Initialise()
        {
        }
        public virtual void InitialiseResources()
        {
        }
        public abstract void Draw();
        protected virtual string ViewModelName => this.GetType().Name + "Model";

        protected virtual object LocateViewModel()
        {
            object viewModel = null;

            if (componentContext.IsRegisteredWithName<object>(this.ViewModelName))
            {
                viewModel = this.componentContext.ResolveNamed<object>(this.ViewModelName);
            }
            return (viewModel);
        }
        protected virtual void OnViewModelChanged(object sender, ChangeNotificationEventArgs<object> e)
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
        protected virtual void CreateBindings()
        {
            if (this.ViewModel?.Value != null)
            {
                foreach (var binding in BindingFactory.CreateViewViewModelBindings(this, this.viewHierarchyPath, this.ViewModel.Value))
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
        /// <summary>
        /// Static method which gets me around CS1540 - trying to access protected virtual methods from
        /// a derived class via a Base class reference.
        protected static void AddToViewHierarchyPath(View view, string path)
        {
            view.AddToViewHierarchyPath(path);
        }
        /// <summary>
        /// Same deal - static method which gets me around CS1540
        /// </summary>
        protected static string GetViewHierarchyPath(View view) => view.ViewHierarchyPath;

        IComponentContext componentContext;
        string viewHierarchyPath;
        static readonly string hierarchyPathSeparator = "/";
    }
}