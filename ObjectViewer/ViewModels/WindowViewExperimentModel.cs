using ObjectViewer.BindingFramework;
using StereoKit;

namespace ObjectViewer.ViewModels
{
    internal class WindowViewExperimentModel
    {
        public Notifiable<string> Title { get; set; } = new Notifiable<string>();
        public Notifiable<Pose> Pose { get; set; } = new Notifiable<Pose>();
        public Notifiable<Vec2> Size { get; set; } = new Notifiable<Vec2>();
        public Notifiable<UIWin> WindowType { get; set; } = new Notifiable<UIWin>();
        public Notifiable<UIMove> MoveType { get; set; } = new Notifiable<UIMove>();

        public ButtonViewModel OkButton { get; set; } = new ButtonViewModel();

        public WindowViewExperimentModel()
        {
            this.Title.SetValue("Window Title");
            this.Pose.SetValue(new Pose(new Vec3(0, 0, -0.5f), Quat.LookAt(new Vec3(0, 0, -0.5f), new Vec3(0, 0, 0))));
            this.Size.SetValue(new Vec2(0.8f, 0.4f));
            this.WindowType.SetValue(UIWin.Normal);
            this.MoveType.SetValue(UIMove.FaceUser);

            this.OkButton.Text.SetValue("OK");
            this.OkButton.Size.SetValue(new Vec2(0.2f, 0.1f));
            this.OkButton.Position.SetValue(new Vec3(0, 0, 0));
        }
    }
}
