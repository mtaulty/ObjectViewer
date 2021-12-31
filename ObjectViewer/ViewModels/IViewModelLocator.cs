using Autofac;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace ObjectViewer.ViewModels
{
    internal interface IViewModelLocator
    {
        bool HasViewModel(string viewModelName);
        object GetViewModelInstance(string viewModelName);
    }
}
