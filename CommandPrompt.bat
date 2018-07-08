@CD /D "%~dp0"
@title Lucid Command Prompt
@SET PATH=C:\Program Files\dotnet\;%PATH%
@SET PATH=%ProgramFiles(x86)%\Microsoft Visual Studio\2017\Enterprise\MSBuild\15.0\Bin\;%ProgramFiles(x86)%\Microsoft Visual Studio\2017\Professional\MSBuild\15.0\Bin\;%ProgramFiles(x86)%\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\;%ProgramFiles(x86)%\Microsoft Visual Studio\2017\BuildTools\MSBuild\15.0\Bin\;%PATH%
type readme.txt
@doskey bc=dotnet clean build.proj
@doskey btw=dotnet watch msbuild build.proj /p:FilterTestFqn=$1 $2 $3 $4 $5 $6 $7 $8 $9
@doskey bt=dotnet msbuild build.proj /p:FilterTestFqn=$1 $2 $3 $4 $5 $6 $7 $8 $9
@doskey bw=dotnet watch msbuild build.proj $*
@doskey b=dotnet msbuild build.proj $*
@doskey br=dotnet restore build.proj $*
@echo.
@echo Aliases:
@echo.
@doskey /MACROS
%comspec%
