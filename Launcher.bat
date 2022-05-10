@echo off
echo First Task Here ...

timeout /t 20 

IF EXIST "SYKO_Systeme-solaire.exe" (
    pushd %~dp0
	SYKO_Systeme-solaire.exe
	popd
) ELSE (
  echo EXE not found, please put this bat file in same folder as executable
)




Exit