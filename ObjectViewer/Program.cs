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
            builder.RegisterType<WindowView>().AsSelf();
            builder.RegisterType<ButtonView>().AsSelf();
            builder.RegisterType<WindowViewModel>().Named<object>(nameof(WindowViewModel));
            container = builder.Build();
        }
        static void Draw()
        {
            var drawables = new View[]
            {
                container.Resolve<WindowView>()
            };

            drawables.ForAll(d => d.InitialiseResources());

            while (SK.Step(
                () =>
                {
                    drawables.ForAll(d =>
                        {
                            d.BeginDraw();
                            d.Draw();
                            d.EndDraw();
                        }
                    );
                }
            )) ;
        }
        static IContainer container;
    }
}