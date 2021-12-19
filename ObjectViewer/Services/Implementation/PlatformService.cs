using ObjectViewer.Services.Definitions;
using StereoKit;

namespace ObjectViewer.Services.Implementation
{
    internal class PlatformService : IPlatformService
    {
        public bool IsOpaqueDisplay => SK.System.displayType == Display.Opaque;
    }
}