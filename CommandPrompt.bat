@CD /D "%~dp0"
@title Lucid Command Prompt
@SET PATH=C:\Program Files\dotnet\;%PATH%
type readme.txt
@doskey bc=dotnet clean
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
