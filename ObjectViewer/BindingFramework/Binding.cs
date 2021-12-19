namespace ObjectViewer.BindingFramework
{
    internal class Binding<T>  : IBinding
    {
        public Binding(Notifiable<T> view, Notifiable<T> viewModel)
        {
            this.view = view;
            this.viewModel = viewModel;
        }
        public void AddHandlers()
        {
            this.view.Changed += OnViewChanged;
            this.viewModel.Changed += OnViewModelChanged;
        }
        public void ViewToViewModel()
        {
            this.viewModel.SetValue(this.view.Value, false);
        }
        public void ViewModelToView()
        {
            this.view.SetValue(this.viewModel.Value, false);
        }
        void OnViewModelChanged(object sender, ChangeNotificationEventArgs<T> e)
        {
            this.ViewModelToView();
        }
        void OnViewChanged(object sender, ChangeNotificationEventArgs<T> e)
        {
            this.ViewToViewModel();
        }
        public void RemoveHandlers()
        {
            this.view.Changed -= OnViewChanged;
            this.viewModel.Changed -= OnViewModelChanged;
        }
        Notifiable<T> view;
        Notifiable<T> viewModel;
    }
}