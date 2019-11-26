using System;
using System.Collections.Generic;

namespace ShUtilities.Common
{
    /// <summary>
    /// Disposes one or more <see cref="IDisposable"/>s and sets their references to null
    /// </summary>
    public static class Disposer
    {
        public static void Null<T>(ref T disposable)
            where T: class, IDisposable
        {
            if (disposable != null)
            {
                disposable.Dispose();
                disposable = null;
            }
        }

        public static T Null<T>(T disposable)
            where T: class, IDisposable
        {
            disposable?.Dispose();
            return null;
        }

        public static void All<TCollection>(TCollection disposables)
            where TCollection: IEnumerable<IDisposable>
        {
            foreach (IDisposable item in disposables)
            {
                item.Dispose();
            }
        }

        public static void AllNull<TCollection>(ref TCollection disposables)
            where TCollection: class, IEnumerable<IDisposable>
        {
            if (disposables != null)
            {
                foreach (IDisposable item in disposables)
                {
                    item.Dispose();
                }
                disposables = null;
            }
        }
    }
}
