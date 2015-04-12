@CD /D "%~dp0"
@title Lucid Command Prompt
@SET PATH=C:\WINDOWS\Microsoft.NET\Framework\v4.0.30319\;%PATH%
@doskey b=msbuild $* Build.proj
type readme.txt
%comspec%
