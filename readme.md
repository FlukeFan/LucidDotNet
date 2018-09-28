
[![Build Status](https://ci.appveyor.com/api/projects/status/github/FlukeFan/LucidDotNet?svg=true)](https://ci.appveyor.com/project/FlukeFan/LucidDotNet) <pre>

Lucid
=====

An example application using ASP.Net Core.  https://lucid.rgbco.uk

Building
========

To build, open CommandPrompt.bat as administrator, and type 'br' (to restore) then 'b'.

Build commands:

br                                      Restore dependencies (execute this first)
b                                       Dev-build
ba                                      Build all (including slow tests and coverage)
bw                                      Watch dev-build
bt [test]                               Run tests with filter Name~[test]
btw [test]                              Watch run tests with filter Name~[test]
bc                                      Clean the build outputs

mvc                                     Run the MVC site
mvcw                                    Watch run the MVC site
