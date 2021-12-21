using Autofac;
using ObjectViewer.BindingFramework;
using System.Collections.Specialized;

namespace ObjectViewer.Views
{
    internal class WindowTwoButtonView : View
    {
        public Notifiable<string> Title { get; set; }
        public ButtonView OKButton { get; set; }
        public ButtonView CancelButton { get; set; }

        public WindowTwoButtonView(IComponentContext componentContext) : base(componentContext)
        {
        }
    }
}
