MSBuild.exe tmac.sln /p:Configuration=Debug /p:Platform="Any CPU"
if errorlevel 1 goto :eof

bin\Debug\tmac -h

echo file is $file >%temp%\test.txt
echo pattern >>%temp%\test.txt
bin\Debug\tmac -r pattern replacement %temp%\test.txt
type %temp%\test.txt
