﻿using Autofac;
using ObjectViewer.BindingFramework;
using StereoKit;

namespace ObjectViewer.Views
{
    internal class WindowView : ContainerView
    {
        public Notifiable<string> Title { get; set; } = new Notifiable<string>();
        public Notifiable<Pose> Pose { get; set; } = new Notifiable<Pose>();
        public Notifiable<Vec2> Size { get; set; } = new Notifiable<Vec2>();
        public Notifiable<UIWin> WindowType { get; set; } = new Notifiable<UIWin>();
        public Notifiable<UIMove> MoveType { get; set; } = new Notifiable<UIMove>();

        public WindowView(IComponentContext componentContext) : base(componentContext)
        {
        }
        public override void Initialise(IComponentContext componentContext)
        {
            base.AddChildView("OKBtn", componentContext.Resolve<ButtonView>());
            base.AddChildView("CancelBtn", componentContext.Resolve<ButtonView>());
            base.Initialise(componentContext);
        }
        public override void BeginDraw()
        {
            base.BeginDraw();
            Pose pose = this.Pose;
            UI.WindowBegin(this.Title, ref pose, this.Size, this.WindowType, this.MoveType);
            this.Pose.SetValue(pose);
        }
        public override void EndDraw()
        {
            UI.WindowEnd();
            base.EndDraw();
        }
    }
}