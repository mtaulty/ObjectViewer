using Autofac;
using ObjectViewer.BindingFramework;
using StereoKit;

namespace ObjectViewer.Views
{
    internal class CubeView : View
    {
        [BindsAs("Foo")]
        public Notifiable<Pose> Pose { get; set; } = new Notifiable<Pose>();

        [BindsAs("Bar")]
        public Notifiable<float> Size { get; set; } = new Notifiable<float>();

        public CubeView(IComponentContext componentConext) : base (componentConext)
        {
        }
        public override void Initialise()
        {
            this.cube = Model.FromMesh(Mesh.GenerateCube(Vec3.One * this.Size),Default.MaterialUI);
        }
        public override void Draw()
        {
            Pose pose = this.Pose;

            if (UI.Handle("Cube", ref pose, this.cube.Bounds))
            {
                this.Pose.SetValue(pose);
            }
            this.cube.Draw(this.Pose.Value.ToMatrix());
        }
        Model cube;
    }
}