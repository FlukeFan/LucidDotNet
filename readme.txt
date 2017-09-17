
Lucid

To build, run CommandPrompt.bat (as administrator), then type 'b'

For development in Visual Studio:
    * install 'Web Compiler' to get automatic .scss compilation

Build commands:

b                       : build
b /t:clean              : clean
b /t:RestorePackages    : Restore NuGet packages
b /t:Package            : Package into zip file
b /t:Deploy             : Deploy package
