using System;

namespace ObjectViewer.BindingFramework
{
    class Notifiable<T>
    {
        public event EventHandler<ChangeNotificationEventArgs<T>> Changed;
        public Notifiable()
        {
            this.value = default(T);
        }
        public T Value
        {
            get => this.value;
        }
        public void SetValue(T newValue, bool fireChangeNotification = true)
        {
            if (!Object.Equals(this.value, newValue))
            {
                T oldValue = this.value;
                this.value = newValue;

                if (fireChangeNotification)
                {
                    this.FireChanged(oldValue);
                }
            }
        }
        protected void FireChanged(T oldValue)
        {
            this.Changed?.Invoke(this, new ChangeNotificationEventArgs<T>(oldValue, this.value));
        }
        public static implicit operator T(Notifiable<T> notifiable) => notifiable.Value;
        T value;
    }
}