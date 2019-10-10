# ShUtilities

This is a collection of .NET utilities that I have accumulated over the 10+ years of coding on the .NET platform. I've organized the code into namespaces that correspond to the framework namespaces they are extending.

This is still a work in process as I clean up my code to make it fit for publication here.

## Diagnostics

* __ProcessTuning__: tweak a process's priority allowing it to consume all of a machine's resources as long as there is no other resource demand

## Interop

* __HGlobal__: wrap allocation and deallocation of unmanaged memory into an IDisposable to be used in a using-statement thus avoiding leaks of unmanaged memory caused by forgotten/wrong deallocation
* __Libary__: load a DLL and dynmically get delegates to its functions at runtime