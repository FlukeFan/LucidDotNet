@CD /D "%~dp0"
@title Demo Command Prompt
@SET PATH=C:\WINDOWS\Microsoft.NET\Framework\v4.0.30319\;%PATH%
@doskey b=msbuild $* Demo.proj /p:RestorePackages=True
type readme.txt
%comspec%
