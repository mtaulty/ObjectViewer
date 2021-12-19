namespace ObjectViewer.BindingFramework
{
    internal interface IBinding
    {
        void ViewToViewModel();
        void ViewModelToView();
        void AddHandlers();
        void RemoveHandlers();
    }
}