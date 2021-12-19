using Autofac;
using ObjectViewer.Extensions;
using ObjectViewer.Services.Definitions;
using ObjectViewer.Services.Implementation;
using ObjectViewer.Views;
using StereoKit;
using System;

namespace ObjectViewer.ViewModels
{
    internal class Program
    {
        static void Main(string[] args)
        {
            StereoKitInitialise();
            ContainerInitialise();

            Draw();

            SK.Shutdown();
        }
        static void StereoKitInitialise()
        {
            // Initialize StereoKit
            SKSettings settings = new SKSettings
            {
                appName = "ObjectViewer",
                assetsFolder = "Assets",
            };

            if (!SK.Initialize(settings))
            {
                Environment.Exit(1);
            }
        }
        static void ContainerInitialise()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<PlatformService>().As<IPlatformService>().SingleInstance();
            builder.RegisterType<CubeViewModel>().Named<object>(nameof(CubeViewModel));
            builder.RegisterType<FloorViewModel>().Named<object>(nameof(FloorViewModel));
            builder.RegisterType<CubeView>().AsSelf();
            builder.RegisterType<FloorView>().AsSelf();
            container = builder.Build();
        }
        static void Draw()
        {
            var drawables = new IDrawable[]
            {
                container.Resolve<FloorView>(),
                container.Resolve<CubeView>()
            };

            drawables.ForAll(d => d.Initialise());

            while (SK.Step(
                () => 
                {
                    drawables.ForAll(d => d.Draw());
                }
                )
            ) ;
        }
        static IContainer container;
    }
}