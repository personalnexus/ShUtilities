using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace ShUtilities.Windows
{
    public static class NotifyPropertyChangedExtensions<T>
    {
        internal static FieldInfo PropertyChangedFieldInfo = typeof(T).GetField("PropertyChanged", BindingFlags.Instance | BindingFlags.NonPublic);
    }

    public static class NotifyPropertyChangedExtensions
    {
        public static void Raise<TSender, TValue>(this TSender sender, ref TValue backingField, TValue newValue, [CallerMemberName] string propertyName = null)
            where TSender : INotifyPropertyChanged
        {
            // does not require passing the event delegate, but uses a bunch of Reflection at every call
            if (!EqualityComparer<TValue>.Default.Equals(backingField, newValue))
            {
                backingField = newValue;

                if (NotifyPropertyChangedExtensions<TSender>.PropertyChangedFieldInfo.GetValue(sender) is MulticastDelegate eventDelagate)
                {
                    var eventArgs = new PropertyChangedEventArgs(propertyName);
                    foreach (Delegate @delegate in eventDelagate.GetInvocationList())
                    {
                        @delegate.Method.Invoke(@delegate.Target, new object[] { sender, eventArgs });
                    }
                }
            }
        }
        
        public static void Raise<TValue>(this PropertyChangedEventHandler eventDelegate, INotifyPropertyChanged sender, ref TValue backingField, TValue newValue, [CallerMemberName] string propertyName = null)
        {
            if (!EqualityComparer<TValue>.Default.Equals(backingField, newValue))
            {
                backingField = newValue;
                if (eventDelegate != null)
                {
                    var eventArgs = new PropertyChangedEventArgs(propertyName);
                    eventDelegate?.Invoke(sender, eventArgs);
                }
            }
        }
    }
}