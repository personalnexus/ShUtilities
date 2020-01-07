using System;
using System.Collections.Generic;

namespace ShUtilities.Common
{
    /// <summary>
    /// Disposes one or more <see cref="IDisposable"/>s and sets their references to null
    /// </summary>
    public static class Disposer
    {
        /// <summary>
        /// Disposes <paramref name="disposable"/> if it isn't null and sets the reference to null afterwards.
        /// </summary>
        public static void Null<T>(ref T disposable)
            where T: class, IDisposable
        {
            if (disposable != null)
            {
                IDisposable disposableCopy = disposable;
                disposable = null;
                disposableCopy.Dispose();
            }
        }

        /// <summary>
        /// Disposes <paramref name="disposable"/> if it isn't null and returns null. Use for properties and such that cannot be passed in as ref parameters
        /// </summary>
        /// <example>
        /// MyProperty = Disposer.Null(MyProperty);
        /// </example>
        public static T Null<T>(T disposable)
            where T: class, IDisposable
        {
            disposable?.Dispose();
            return null;
        }

        /// <summary>
        /// Disposes every item in <paramref name="disposables"/>.
        /// </summary>
        public static void All<TCollection>(TCollection disposables)
            where TCollection: IEnumerable<IDisposable>
        {
            foreach (IDisposable item in disposables)
            {
                item.Dispose();
            }
        }

        /// <summary>
        /// Disposes every item in <paramref name="disposables"/> and sets the collection reference to null afterwards.
        /// </summary>
        public static void AllNull<TCollection>(ref TCollection disposables)
            where TCollection : class, IEnumerable<IDisposable>
        {
            if (disposables != null)
            {
                TCollection disposableCopies = disposables;
                disposables = null;
                foreach (IDisposable item in disposableCopies)
                {
                    item.Dispose();
                }
            }
        }
    }
}
