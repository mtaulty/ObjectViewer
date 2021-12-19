using ObjectViewer.Interfaces;
using StereoKit;
using System;

namespace ObjectViewer.Drawables
{
    internal class Floor : IDrawable, IConditional
    {
        public bool IsTrue => SK.System.displayType == Display.Opaque;
        public void Initialise()
        {
            this.floorTransform = Matrix.TS(0, -1.5f, 0, new Vec3(30, 0.1f, 30));
            this.floorMaterial = new Material(Shader.FromFile("floor.hlsl"));
            floorMaterial.Transparency = Transparency.Blend;
        }
        public void Draw()
        {
            Default.MeshCube.Draw(floorMaterial, floorTransform);
        }
        Matrix floorTransform;
        Material floorMaterial;
    }
}
