
[![Build Status](https://ci.appveyor.com/api/projects/status/github/FlukeFan/LucidDotNet?svg=true)](https://ci.appveyor.com/project/FlukeFan/LucidDotNet) <pre>

Lucid
=====

An example application using ASP.Net Core.  https://lucid.rgbco.uk

Building
========

Pre-requisites
--------------

* Docker;
* .NET SDK specified in global.json.


Additional recommendations for Visual Studio
--------------------------------------------

* Visual Studio 2017/2019 (community is fine)
* Bundler & Minifier
* Web Compiler
* Browser Reload on Save


To build:

1. Open CommandPrompt.bat as administrator;
2. Type 'dcud' (docker compose up detach);
3. Type 'br' (restores NuGet packages);
4. Type 'b' to build.

Build commands
--------------

br                                      Restore dependencies (execute this first)
b                                       Dev-build
ba                                      Build all (including slow tests and coverage)
bw                                      Watch dev-build
bt [test]                               Run tests with filter Name~[test]
btw [test]                              Watch run tests with filter Name~[test]
bc                                      Clean the build outputs

dcu                                     Docker compose up
dcud                                    Docker compose up (detach)

web                                     Run the Web application
webw                                    Watch run the Web application
