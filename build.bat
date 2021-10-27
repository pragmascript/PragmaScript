cd src
call dotnet build -c %1 

cd..
cd tools
cd PragmaLangServer
call dotnet publish -c %1 

cd ..
cd ..
cd tools
cd PragmaLangClient
call package.bat %1
call code --install-extension pragmalangclient-0.0.1.vsix
cd ..
cd ..
