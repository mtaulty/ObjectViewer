using System;

namespace ObjectViewer.BindingFramework.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]

    internal class BindsAttribute : Attribute
    {
        public BindsAttribute(string name)
        {
            this.Name = name;
        }
        public string Name { get; set; }
    }
}
