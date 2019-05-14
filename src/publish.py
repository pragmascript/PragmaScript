from shutil import copy, copytree, rmtree, make_archive, move
from subprocess import call
import os
import time
import sys



cwd = os.getcwd()

def nj(*paths):
    return os.path.normpath(os.path.join(*paths)) 
call("dotnet clean -c release")
call("dotnet publish -c release -r win7-x64 /p:PublishSingleFile=true")
binPath = nj(cwd, "bin/Release/netcoreapp3.0/win7-x64/publish")
publishPath = nj(cwd, "../publish/pragma")

copytree(binPath, nj(publishPath, "bin"))
copytree(nj(cwd, "../publish/current/bin/external"), nj(publishPath, "bin/external"))

def ignore(path, names):
    ignored = []
    for x in names:
        xpath = nj(path, x)
        if (x == "bin" or x == "obj" or x == "cpp") and (os.path.isdir(xpath)):
            ignored.append(x)
    return ignored

copytree(nj(cwd, "../publish/current/samples"), nj(publishPath, "samples"), ignore=ignore)
archive_path = nj(cwd, "../publish", time.strftime("pragma_%Y_%m_%d"))
make_archive(archive_path, "zip", nj(cwd, "../publish"), "pragma")
move(archive_path+".zip", nj(cwd, "../publish/releases",))
rmtree(publishPath)
