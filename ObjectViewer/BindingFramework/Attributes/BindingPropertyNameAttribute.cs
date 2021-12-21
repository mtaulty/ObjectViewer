using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectViewer.BindingFramework
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    internal class BindingPropertyNameAttribute : Attribute
    {
        public BindingPropertyNameAttribute(string name)
        {
            this.Name = name;
        }
        public string Name { get; set; }
    }
}
