msbuild AppHarbor\AppHarbor.proj /t:Push
@IF ERRORLEVEL 1 GOTO ERROR

git push appharbor master
@IF ERRORLEVEL 1 GOTO ERROR

@echo Success
@GOTO FINALLY

:ERROR
@echo ERROR

:FINALLY
