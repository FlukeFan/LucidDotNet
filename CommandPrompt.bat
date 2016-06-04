@CD /D "%~dp0"
@title Lucid Command Prompt
@SET PATH=C:\Program Files (x86)\MSBuild\14.0\Bin\;%PATH%
@doskey b=msbuild $* Lucid.proj
type readme.txt
%comspec%
