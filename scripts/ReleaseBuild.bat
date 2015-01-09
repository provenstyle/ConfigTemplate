rem clean
del ..\src\bin\Release /f /q
del ..\release\ /f /q

rem build
msbuild ../src/ConfigTemplate.csproj /t:Build /p:Configuration=Release

rem bundle dlls
..\src\packages\ilmerge.2.14.1208\tools\ILMerge.exe /target:exe /targetplatform:"v4,C:\Windows\Microsoft.NET\Framework\v4.0.30319" /out:..\release\ConfigTemplate.exe ..\src\bin\Release\ConfigTemplate.exe ..\src\bin\Release\DotLiquid.dll ..\src\bin\Release\Newtonsoft.Json.dll