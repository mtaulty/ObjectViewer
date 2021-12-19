using ObjectViewer.Drawables;
using ObjectViewer.Interfaces;
using ObjectViewer.Views;
using StereoKit;
using System;
using System.Collections.Generic;

namespace ObjectViewer.ViewModels
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Initialise();

            Draw();

            SK.Shutdown();
        }
        static void Initialise()
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
        static void Draw()
        {
            var cubeView = new CubeView();
            var viewModel = new CubeViewModel();
            cubeView.ViewModel.SetValue(viewModel);

            var drawables = new IDrawable[]
            {
                new Floor(),
                cubeView
            };

            ConditionalForAll(drawables, d => d.Initialise());

            while (SK.Step(
                () =>
                    {
                        ConditionalForAll(drawables, d => d.Draw());
                    }
                )
            ) ;
        }
        static void ConditionalForAll<T>(IEnumerable<T> list, Action<T> action)
        {
            foreach (var item in list)
            {
                var condition = item as IConditional;

                if ((condition == null) || condition.IsTrue)
                {
                    action(item);
                }
            }
        }
    }
}
