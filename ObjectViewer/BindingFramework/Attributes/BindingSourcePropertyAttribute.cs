using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectViewer.BindingFramework
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    internal class BindingSourcePropertyAttribute : Attribute
    {
        public BindingSourcePropertyAttribute(string name)
        {
            this.Name = name;
        }
        public string Name { get; set; }
    }
}
