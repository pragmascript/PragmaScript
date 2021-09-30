mkdir server
del server\*.* /Q
copy ..\PragmaLangServer\bin\%1\net5.0\win7-x64\publish\*.* server\
vsce package