using ObjectViewer.BindingFramework;
using StereoKit;

namespace ObjectViewer.ViewModels
{
    internal class WindowViewModel
    {
        public Notifiable<string> Title { get; set; } = new Notifiable<string>();
        public Notifiable<Pose> Pose { get; set; } = new Notifiable<Pose>();
        public Notifiable<Vec2> Size { get; set; } = new Notifiable<Vec2>();
        public Notifiable<UIWin> WindowType { get; set; } = new Notifiable<UIWin>();
        public Notifiable<UIMove> MoveType { get; set; } = new Notifiable<UIMove>();
        public WindowViewModel()
        {
            this.Title.SetValue("Window Title");
            this.Pose.SetValue(new Pose(new Vec3(0, 0, -0.5f), Quat.Identity));
            this.Size.SetValue(new Vec2(0.1f, 0.1f));
            this.WindowType.SetValue(UIWin.Normal);
            this.MoveType.SetValue(UIMove.FaceUser);
        }
    }
}
