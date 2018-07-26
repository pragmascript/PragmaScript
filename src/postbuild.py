from shutil import copy
import os
import sys




if sys.argv[1] == "-release":
    prefixPath = "bin/release"
elif sys.argv[1] == "-debug":
    prefixPath = "bin/debug"
else:
    assert false
    



cwd = os.getcwd()


def nj(*paths):
    return os.path.normpath(os.path.join(*paths)) 

binPath = nj(cwd, prefixPath, "netcoreapp2.1/win7-x64")
publishPath = nj(cwd, "../publish/current/bin")

for _, _, files in os.walk(binPath):
    for f in files:
        fp = nj(binPath, f)
        copy(fp, publishPath)
    break
    