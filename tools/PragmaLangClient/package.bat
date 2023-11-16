mkdir server
del server\*.* /Q
copy ..\PragmaLangServer\bin\%1\net8.0\win-x64\publish\*.* server\
vsce package