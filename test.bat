MSBuild.exe tmac.sln /p:Configuration=Debug /p:Platform="Any CPU"
if errorlevel 1 goto :eof

bin\Debug\tmac -h
bin\Debug\tmac -v

echo file is $file >test.txt
echo pattern >>test.txt
bin\Debug\tmac -r pattern replacement test.txt
type test.txt
