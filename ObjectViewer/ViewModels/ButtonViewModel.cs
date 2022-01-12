using ObjectViewer.BindingFramework;
using StereoKit;

namespace ObjectViewer.ViewModels
{
    internal class ButtonViewModel 
    {
        public Notifiable<string> Text { get; set; } = new Notifiable<string>();
        public Notifiable<Vec2> Size { get; set; } = new Notifiable<Vec2>();
        public Notifiable<Vec3> Position { get; set; } = new Notifiable<Vec3>();
    }
}
