using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace ShUtilities.Windows
{
    public static class NotifyPropertyChangedExtensions
    {
        /// <summary>
        /// Changes a property and raises the PropertyChanged event without one having to pass in the event handler by using reflection during each call
        /// </summary>
        public static void SetAndRaise<TSender, TValue>(this TSender sender, ref TValue backingField, TValue newValue, [CallerMemberName] string propertyName = null)
            where TSender : INotifyPropertyChanged
        {
            if (!EqualityComparer<TValue>.Default.Equals(backingField, newValue))
            {
                backingField = newValue;

                if (NotifyPropertyChangedExtensions<TSender>.PropertyChangedFieldInfo.GetValue(sender) is PropertyChangedEventHandler eventHandler)
                {
                    var eventArgs = new PropertyChangedEventArgs(propertyName);
                    eventHandler(sender, eventArgs);
                }
            }
        }

        /// <summary>
        /// Changes a property and raises the given PropertyChanged event without using reflection during each call
        /// </summary>
        public static void SetAndRaise<TSender, TValue>(this TSender sender, ref TValue backingField, TValue newValue, PropertyChangedEventHandler eventHandler, [CallerMemberName] string propertyName = null)
            where TSender : INotifyPropertyChanged
        {
            if (!EqualityComparer<TValue>.Default.Equals(backingField, newValue))
            {
                backingField = newValue;
                if (eventHandler != null)
                {
                    var eventArgs = new PropertyChangedEventArgs(propertyName);
                    eventHandler(sender, eventArgs);
                }
            }
        }
    }

    internal static class NotifyPropertyChangedExtensions<T>
    {
        /// <summary>
        /// Caches the field info to get the PropertyChanged event handler by type
        /// </summary>
        internal static FieldInfo PropertyChangedFieldInfo = typeof(T).GetField(nameof(INotifyPropertyChanged.PropertyChanged), BindingFlags.Instance | BindingFlags.NonPublic);
    }
}