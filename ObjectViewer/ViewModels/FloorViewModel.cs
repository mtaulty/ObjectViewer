using ObjectViewer.BindingFramework;
using ObjectViewer.Services.Definitions;

namespace ObjectViewer.ViewModels
{
    internal class FloorViewModel
    {
        public Notifiable<bool> DrawFloor { get; set; } = new Notifiable<bool>();

        public FloorViewModel(IPlatformService platformService)
        {
            this.DrawFloor.SetValue(platformService.IsOpaqueDisplay);
        }
    }
}
