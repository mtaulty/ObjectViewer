using Autofac;

namespace ObjectViewer.ViewModels
{
    internal class ComponentContextViewModelLocator : IViewModelLocator
    {
        public IComponentContext ComponentContext => this.componentContext;
        public ComponentContextViewModelLocator(IComponentContext componentContext)
        {
            this.componentContext = componentContext;
        }
        public bool HasViewModel(string viewModelName)
        {
            return(this.componentContext.IsRegisteredWithName<object>(viewModelName));
        }
        public object GetViewModelInstance(string viewModelName)
        {
            return (this.componentContext.ResolveNamed<object>(viewModelName));
        }
        IComponentContext componentContext;

    }
}
