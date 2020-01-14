﻿# ShUtilities

This is a collection of .NET utilities that I have accumulated over the 10+ years of coding on the .NET platform. I've organized the code into namespaces that roughly correspond to the framework namespaces they are extending.

This is still a work in process as I clean up my code to make it fit for publication here.

## Collections

* __ByteDictionary__: provides IDictionary-like access with a fixed memory-footprint to what is essentially an array with a byte as its index
* __CollectionExtensions__: extension methods for ICollection<>
* __DictionaryDiff__: compares two dictionaries key by key and stores the added, removed etc. keys and values
* __DictionaryExtensions__: extension methods for IDictionary<,>
* __EnumerableExtensions__: extension methods IEnumerable<>
* __ReverseComparer__: reverses the comparison result of the given comparer
* __SortedListExtensions__: extension methods SortedList<>
* __Trie__: a Trie implementation with support for a reduced set of possible key elements to minimize space requirements that implements most of IDictionary<string, TValue>
* __TrieCharacterSets__: pre-defined sets of characters for Trie e.g. all (Latin) alpha-numeric characters

## Common

* __Bytes__: working with counts of bytes at different scales in a way inspired by TimeSpan
* __Disposer__: disposes one or more IDisposables and sets their references to null
* __MathExtensions__: extension methods adding math functions to built-in value types

## Diagnostics

* __ProcessTuning__: tweak a process's priority allowing it to consume all of a machine's resources as long as there is no other resource demand

## Interop

* __HGlobal__: wrap allocation and deallocation of unmanaged memory into an IDisposable to be used in a using-statement thus avoiding leaks of unmanaged memory caused by forgotten/wrong deallocation
* __Library__: load a DLL and dynamically get delegates to its functions at runtime

## IO

* __FileUtility__: read file line by line as an IEnumerable<string> like File.ReadLines but with control over file open, access and share modes
* __ISerializer__: basic interface describing serialization e.g. XML, JSON, CSV...
* __JsonSerializer__ : implementation of ISerializer<T> for JSON
* __SerializerExtensions__: extension methods for the basic ISerializer<T> interface.
* __TemporaryFile__: creates a temporary file and makes sure it is deleted using the Disposable pattern
* __XmlSerializer__ : implementation of ISerializer<T> for XML
* __XmlUtility__: helper methods for working with XML

## Net

*  __WebClientEx__: derived from WebClient this class adds convient configuration of a proxy as well as other little niceties.

## Text

* __DictionaryParserExtensions__: provides extension methods for getting values from a dictionary and applying a parser/converter to them, e.g. to get an integer from a string-string-dictionary.
* __Parsers__: a repository of parser methods to parse/convert from string to a target type
* __StringExtensions__: convenience methods for common string operations

## Threading

* __ActorSynchronizationContext__: an implementation of a SynchronizationContext that represents an actor in the Actor Pattern that may have changes applied to it via callbacks posted to it and then updates its externally visible state at once based on the one or more updates made to it via the callbacks.
* __SetOnceEvent__: a light-weight alternative to ManualResetEvent for events that are only set once that does not need to be disposed
* __SynchronizationContextExtensions__: extension methods for SynchronizationContext implementations
* __SynchronizationContextSetter__: uses the Dispose pattern to set the current SynchronizationContext to the given one and reset it afterwards to the previous one
* __TaskUtility__: helper methods for tasks (TPL)
* __ThreadSwitch__: switches threads (ab)using the Awaitable-Awaiter Pattern inspired by Raymond Chen

### Pooling

* __BagPool__: An implementation of IPool that uses a ConcurrentBag as the underlying storage
* __IPool__: Describes a pool from which objects can be acquired and released
* __IPoolDiagnostics__: Extended information about the internals of a pool for diagnostic purposes
* __PooledObject__: A ref-struct representing an object from a pool that automatically releases the object when disposed
* __PoolExtensions__: Extension methods for IPool
* __QueuePool__: An implementation of IPool that uses a ConcurrentQueue as the underlying storage
* __SingleObjectPool__: An implementation of IPool which provides Thread-safe access to a single pooled object

## Time

* __BusinessCalendar__: default implementation of IBusinessCalendar containing the given holidays (or none if the default constructor is used)
* __BusinessCalendarExtensions__: extension methods for DateTime using information from an IBusinessCalendar e.g. AddBusinessDays() similar to AddDays()
* __DateExtenstions__: convenience methods for comparing DateTime
* __IBusinessCalendar__: an interface that provides information on whether a given date is a business day or not
* __ITimeProvider__: an interface to obtain the current time which can be swapped out for tests to simulate progression
* __SystemTimeProvider__: implementation of ITimeProvider that provides the time from DateTime.Now and ticks via QueryPerformanceCounter

---

# ShUtilitiesTest
* __DictionaryAssert__: assertions for dictionaries
* __ExceptionUtility__: makes testing with expected exceptions easier by allowing to test specific pieces of code (i.e. Actions) for expected exceptions
* __PerformanceUtility__: basic performance tests using Systen.Diagnostics.Stopwatch
* __TestTimeProvider__: an implementation of ITimeProvider whose time can be set to simulate progression of time in tests