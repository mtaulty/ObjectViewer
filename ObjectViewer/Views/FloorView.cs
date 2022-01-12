using Autofac;
using ObjectViewer.BindingFramework;
using ObjectViewer.ViewFramework;
using ObjectViewer.ViewModels;
using StereoKit;

namespace ObjectViewer.Views
{
    internal class FloorView : View
    {
        public Notifiable<bool> DrawFloor { get; set; } = new Notifiable<bool>();

        public FloorView(IComponentContext componentContext, IViewModelLocator viewModelLocator) : base(componentContext, viewModelLocator)
        {
        }
        public override void InitialiseResources()
        {
            base.InitialiseResources();

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