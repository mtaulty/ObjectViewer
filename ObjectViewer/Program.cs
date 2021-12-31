using Autofac;
using ObjectViewer.Extensions;
using ObjectViewer.Services.Definitions;
using ObjectViewer.Services.Implementation;
using ObjectViewer.ViewFramework;
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
            builder.RegisterType<ComponentContextViewModelLocator>().As<IViewModelLocator>();
            builder.RegisterType<PlatformService>().As<IPlatformService>().SingleInstance();
            builder.RegisterType<WindowViewExperiment>().AsSelf();
            builder.RegisterType<ButtonView>().AsSelf();
            builder.RegisterType<WindowViewExperimentModel>().Named<object>(nameof(WindowViewExperimentModel));
            builder.RegisterType<FloorView>().AsSelf();
            builder.RegisterType<FloorViewModel>().AsSelf();
            container = builder.Build();
        }
        static void Draw()
        {
            var views = new View[]
            {
                container.Resolve<WindowViewExperiment>(),
                container.Resolve<FloorView>()
            };

            views.ForAll(v => v.InitialiseViewModelAndBind());

            views.ForAll(v => v.InitialiseResources());

            while (SK.Step(() => views.ForAll(v => v.Draw()))) ;
        }
        static IContainer container;
    }
}