MSBuild.exe tmac.sln /p:Configuration=Debug /p:Platform="Any CPU"
if errorlevel 1 goto :eof

echo foo $file bar >%temp%\test.txt
bin\Debug\tmac %temp%\test.txt
type %temp%\test.txt
