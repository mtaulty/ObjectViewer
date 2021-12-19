using ObjectViewer.BindingFramework;
using StereoKit;

namespace ObjectViewer.ViewModels
{
    internal class CubeViewModel
    {
        public CubeViewModel()
        {
            this.Pose.SetValue(new Pose(new Vec3(0, 0, -1), Quat.Identity));
            this.Size.SetValue(0.1f);
        }
        public Notifiable<Pose> Pose { get; set; } = new Notifiable<Pose>();
        public Notifiable<float> Size { get; set; } = new Notifiable<float>();
    }
}