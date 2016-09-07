@CD /D "%~dp0"
@title Demo Command Prompt
@SET PATH=C:\Program Files (x86)\MSBuild\14.0\Bin\;%PATH%
@doskey b=msbuild $* Demo.proj
type readme.txt
%comspec%
