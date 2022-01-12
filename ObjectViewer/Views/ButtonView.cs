using Autofac;
using ObjectViewer.BindingFramework;
using ObjectViewer.ViewFramework;
using ObjectViewer.ViewModels;
using StereoKit;

namespace ObjectViewer.Views
{
    internal class ButtonView : View
    {
        public Notifiable<string> Text { get; set; } = new Notifiable<string>();
        public Notifiable<bool> IsRound { get; set; } = new Notifiable<bool>();
        public Notifiable<Vec2> Size { get; set; } = new Notifiable<Vec2>();
        public Notifiable<Vec3> Position { get; set; } = new Notifiable<Vec3>();

        public ButtonView(IComponentContext ctx, IViewModelLocator viewModelLocator) : base(ctx, viewModelLocator)
        {
            this.Text.SetValue("Default");
            this.Position.SetValue(Vec3.Zero);
            this.Size.SetValue(Vec2.Zero);
            this.IsRound.SetValue(false);
        }
        public override void Draw()
        {
            UI.ButtonAt(this.Text, this.Position, this.Size);
        }
    }
}
