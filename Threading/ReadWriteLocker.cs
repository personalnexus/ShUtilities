﻿using System;
using System.Threading;

namespace ShUtilities.Threading
{
    public static class ReadWriteLocker
    {
        /// <summary>
        /// Places a new instance of <typeparamref name="T"/> in a struct with a ReaderWriterLockSlim to provide convenient Read() and Write() methods that acquire/release the corresponding lock via the Dispose pattern
        /// </summary>
        public static ReadWriteLocker<T> Create<T>()
            where T : new()
        {
            return new ReadWriteLocker<T>(new T());
        }

        /// <summary>
        /// Places the given instance of <typeparamref name="T"/> in a struct with a ReaderWriterLockSlim to provide convenient Read() and Write() methods that acquire/release the corresponding lock via the Dispose pattern
        /// </summary>
        public static ReadWriteLocker<T> Create<T>(T value)
        {
            return new ReadWriteLocker<T>(value);
        }
    }

    public struct ReadWriteLocker<T>
    {
        internal ReadWriteLocker(T value)
        {
            Lock = new ReaderWriterLockSlim();
            Value = value;
        }

        public ReaderWriterLockSlim Lock { get; }
        public T Value { get; }

        public static implicit operator T(ReadWriteLocker<T> locker) => locker.Value;

        public ReadWriteLockerReader Read() => new ReadWriteLockerReader(Lock);
        public ReadWriteLockerWriter Write() => new ReadWriteLockerWriter(Lock);


        public struct ReadWriteLockerReader : IDisposable
        {
            private readonly ReaderWriterLockSlim _lock;

            internal ReadWriteLockerReader(ReaderWriterLockSlim @lock)
            {
                _lock = @lock;
                @lock.EnterReadLock();
            }

            public void Dispose() => _lock.ExitReadLock();
        }

        public struct ReadWriteLockerWriter : IDisposable
        {
            private readonly ReaderWriterLockSlim _lock;

            internal ReadWriteLockerWriter(ReaderWriterLockSlim @lock)
            {
                _lock = @lock;
                @lock.EnterWriteLock();
            }

            public void Dispose() => _lock.ExitWriteLock();
        }
    }
}