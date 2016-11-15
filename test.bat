MSBuild.exe tmac.sln /p:Configuration=Debug /p:Platform="Any CPU"
if errorlevel 1 goto :eof
bin\Debug\tmac  -h
