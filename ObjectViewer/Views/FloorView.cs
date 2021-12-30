using Autofac;
using ObjectViewer.BindingFramework;
using ObjectViewer.ViewFramework;
using StereoKit;

namespace ObjectViewer.Views
{
    internal class FloorView : View
    {
        public Notifiable<bool> DrawFloor { get; set; } = new Notifiable<bool>();

        public FloorView(IComponentContext componentContext) : base(componentContext)
        {
        }
        public override void Initialise()
        {
            if (this.DrawFloor.Value)
            {
                this.floorTransform = Matrix.TS(0, -1.5f, 0, new Vec3(30, 0.1f, 30));
                this.floorMaterial = new Material(Shader.FromFile("floor.hlsl"));
                floorMaterial.Transparency = Transparency.Blend;
            }
        }
        public override void Draw()
        {
            if (this.DrawFloor.Value)
            {
                Default.MeshCube.Draw(floorMaterial, floorTransform);
            }
        }
        Matrix floorTransform;
        Material floorMaterial;
    }
}