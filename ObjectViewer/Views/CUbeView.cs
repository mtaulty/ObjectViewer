using ObjectViewer.BindingFramework;
using ObjectViewer.Views;
using StereoKit;

namespace ObjectViewer.Views
{
    internal class CubeView : View
    {
        public Notifiable<Pose> Pose { get; set; } = new Notifiable<Pose>();
        public Notifiable<float> Size { get; set; } = new Notifiable<float>();

        public CubeView()
        {
        }
        public override void Initialise()
        {
            this.cube = Model.FromMesh(
                Mesh.GenerateRoundedCube(Vec3.One * this.Size.Value, 0.02f),
                Default.MaterialUI);
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