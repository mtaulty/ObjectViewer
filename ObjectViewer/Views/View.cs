using Autofac;
using ObjectViewer.BindingFramework;
using System.Collections.Generic;

namespace ObjectViewer.Views
{
    class View
    {
        [BindingHidden]
        public Notifiable<object> ViewModel { get; }

        protected List<IBinding> Bindings { get; set; }

        public View(IComponentContext componentContext)
        {
            this.Bindings = new List<IBinding>();
            this.ViewModel = new Notifiable<object>();
            this.ViewModel.Changed += this.OnViewModelChanged;
            this.Initialise(componentContext);
            this.LocateDirectViewModel(componentContext);
            this.CreateBindings();
        }
        public virtual void Initialise(IComponentContext componentContext)
        {
        }
        public virtual void InitialiseResources()
        {
        }
        public virtual void Draw()
        {
        }
        public virtual void BeginDraw()
        {

        }
        public virtual void EndDraw()
        {

        }
        protected virtual string ViewModelName => this.GetType().Name + "Model";

        protected virtual void LocateDirectViewModel(IComponentContext componentContext)
        {
            if (componentContext.IsRegisteredWithName<object>(this.ViewModelName))
            {
                this.ViewModel.SetValue(componentContext.ResolveNamed<object>(this.ViewModelName));
            }
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
                foreach (var binding in BindingFactory.CreateViewViewModelBindings(this, this.ViewModel.Value))
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
    }
}