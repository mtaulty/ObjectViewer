using ObjectViewer.BindingFramework;
using StereoKit;

namespace ObjectViewer.ViewModels
{
    internal class CubeViewModel
    {
        [BindingSourceProperty("Foo")]
        public Notifiable<Pose> CubePose { get; set; } = new Notifiable<Pose>();
        [BindingSourceProperty("Bar")]
        public Notifiable<float> CubeSize { get; set; } = new Notifiable<float>();

        public CubeViewModel()
        {
            this.CubePose.SetValue(new Pose(new Vec3(0, 0, -1), Quat.Identity));
            this.CubeSize.SetValue(0.1f);
        }
    }
}