from shutil import copy, copytree, rmtree, make_archive
from subprocess import call
import os
import time

cwd = os.getcwd()
print(cwd)

def nj(*paths):
    return os.path.normpath(os.path.join(*paths)) 
call("dotnet clean PragmaCore.csproj -c release")
call("dotnet publish PragmaCore.csproj -c release -r win7-x64")
binPath = nj(cwd, "bin/release/netcoreapp2.0/win7-x64")
publishPath = nj(cwd, "../publish/pragma")

copytree(binPath, nj(publishPath, "bin"))
copytree(nj(cwd, "bin/external"), nj(publishPath, "bin/external"))

def ignore(path, names):
    ignored = []
    for x in names:
        xpath = nj(path, x)
        if (x == "bin" or x == "obj" or x == "cpp") and (os.path.isdir(xpath)):
            ignored.append(x)
    return ignored

copytree(nj(cwd, "samples"), nj(publishPath, "samples"), ignore=ignore)
archive_name = nj(cwd, "publish", time.strftime("pragma_%Y_%m_%d"))
make_archive(archive_name, "zip", nj(cwd, "publish"), "pragma")
rmtree(publishPath)
