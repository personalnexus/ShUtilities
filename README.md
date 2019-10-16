# ShUtilities

This is a collection of .NET utilities that I have accumulated over the 10+ years of coding on the .NET platform. I've organized the code into namespaces that roughly correspond to the framework namespaces they are extending.

This is still a work in process as I clean up my code to make it fit for publication here.

## Collections

* __ByteDictionary__: provides IDictionary-like access with a fixed memory-footprint to what is essentially an array with a byte as its index
* __CollectionExtensions__: extension methods for IDictionary<,> and IEnumerable<>
* __Trie__: a Trie implementation with support for a reduced set of possible key elements to minimize space requirements that implements most of IDictionary<string, TValue>

## Diagnostics

* __ProcessTuning__: tweak a process's priority allowing it to consume all of a machine's resources as long as there is no other resource demand

## Interop

* __HGlobal__: wrap allocation and deallocation of unmanaged memory into an IDisposable to be used in a using-statement thus avoiding leaks of unmanaged memory caused by forgotten/wrong deallocation
* __Library__: load a DLL and dynamically get delegates to its functions at runtime

## IO

* __FileUtility__: read file line by line as an IEnumerable<string> like File.ReadLines but with control over file open, access and share modes
* __ISerializer__: basic interface describing serialization e.g. XML, JSON, CSV...
* __SerializerExtensions__: extension methods for the basic ISerializer<T> interface.
* __JsonSerializer__ : implementation of ISerializer<T> for JSON
* __XmlSerializer__ : implementation of ISerializer<T> for XML

## Text

* __DictionaryParserExtensions__: provides extension methods for getting values from a dictionary and applying a parser/converter to them, e.g. to get an integer from a string-string-dictionary.
* __Parsers__: a repository of parser methods to parse/convert from string to a target type

---

# ShUtilitiesTest

* __ExceptionUtility__: makes testing with expected exceptions easier by allowing to test specific pieces of code (i.e. Actions) for expected exceptions