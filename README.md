# ShowRenamer
A plugin-powered utility for maintaining consistent media file naming

ShowRenamer is released under a 3-clause BSD license. See LICENSE for more details.

## Getting ShowRenamer
At present, no binary releases of ShowRenamer are available for download. If you want to use it, you'll need to build from source.

As ShowRenamer uses some cool stuff from C# 6, building it requires the following prerequisites:
 * Visual Studio 2015 RC (or later)
 * .NET Framework 4.6 RC (or later)

Clone the repository, open the solution file, and hit build. Either copy the built executable to somewhere on your path, or add your build directory to the path.

## Usage
Executing ShowRenamer's executable on the command line without any arguments will make ShowRenamer search and act upon the current working directory `.`.
You can specify an alternative working directory just by providing it as an argument.