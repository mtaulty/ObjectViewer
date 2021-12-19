using System;

namespace ObjectViewer.BindingFramework
{
    class ChangeNotificationEventArgs<T> : EventArgs
    {
        public ChangeNotificationEventArgs(T oldValue, T newValue)
        {
            this.OldValue = oldValue;
            this.NewValue = newValue;
        }
        public T OldValue { get; set; }
        public T NewValue { get; set; }
    }
}