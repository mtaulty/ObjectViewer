using Autofac;
using ObjectViewer.BindingFramework;
using ObjectViewer.ViewFramework;
using StereoKit;

namespace ObjectViewer.Views
{
    internal class WindowViewExperiment : ContainerView
    {
        public Notifiable<string> Title { get; set; } = new Notifiable<string>();
        public Notifiable<Pose> Pose { get; set; } = new Notifiable<Pose>();
        public Notifiable<Vec2> Size { get; set; } = new Notifiable<Vec2>();
        public Notifiable<UIWin> WindowType { get; set; } = new Notifiable<UIWin>();
        public Notifiable<UIMove> MoveType { get; set; } = new Notifiable<UIMove>();

        public ButtonView OkButton { get; set; }
        public ButtonView CancelButton { get; set; }

        public WindowViewExperiment(IComponentContext componentContext) : base(componentContext)
        {
        }
        public override void Initialise()
        {
            base.AddChildView("OKBtn", base.ComponentContext.Resolve<ButtonView>());
            base.AddChildView("CancelBtn", base.ComponentContext.Resolve<ButtonView>());
            base.Initialise();
        }
        protected override void BeginDraw()
        {
            Pose pose = this.Pose;
            UI.WindowBegin(this.Title, ref pose, this.Size, this.WindowType, this.MoveType);
            this.Pose.SetValue(pose);
        }
        protected override void EndDraw()
        {
            UI.WindowEnd();
        }
    }
}