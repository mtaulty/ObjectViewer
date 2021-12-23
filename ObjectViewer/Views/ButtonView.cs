using Autofac;
using ObjectViewer.BindingFramework;
using StereoKit;

namespace ObjectViewer.Views
{
    internal class ButtonView : View
    {
        public Notifiable<string> Text { get; set; } = new Notifiable<string>();
        public Notifiable<bool> IsRound { get; set; } = new Notifiable<bool>();
        public Notifiable<Vec2> Size { get; set; } = new Notifiable<Vec2>();
        public Notifiable<Vec3> Position { get; set; } = new Notifiable<Vec3>();

        public ButtonView(IComponentContext ctx) : base(ctx)
        {
            this.Text.SetValue("Default");
            this.Position.SetValue(Vec3.Zero);
            this.Size.SetValue(Vec2.Zero);
        }
        public override void Initialise()
        {
            base.Initialise();
        }
        public override void Draw()
        {
            UI.ButtonAt(this.Text, this.Position, this.Size);
        }
    }
}
