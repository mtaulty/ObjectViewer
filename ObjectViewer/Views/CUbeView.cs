using Autofac;
using ObjectViewer.BindingFramework;
using StereoKit;

namespace ObjectViewer.Views
{
    internal class CubeView : View
    {
        public Notifiable<Pose> Pose { get; set; } = new Notifiable<Pose>();
        public Notifiable<float> Size { get; set; } = new Notifiable<float>();

        public CubeView(IComponentContext componentConext) : base (componentConext)
        {
        }
        public override void Initialise()
        {
            this.cube = Model.FromMesh(Mesh.GenerateCube(Vec3.One * this.Size.Value),Default.MaterialUI);
        }
        public override void Draw()
        {
            Pose pose = this.Pose.Value;

            if (UI.Handle("Cube", ref pose, this.cube.Bounds))
            {
                this.Pose.SetValue(pose);
            }
            this.cube.Draw(this.Pose.Value.ToMatrix());
        }
        Model cube;
    }
}