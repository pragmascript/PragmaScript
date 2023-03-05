mkdir server
del server\*.* /Q
copy ..\PragmaLangServer\bin\%1\net6.0\win10-x64\publish\*.* server\
vsce package