from shutil import copy
import os

cwd = os.getcwd()

def nj(*paths):
    return os.path.normpath(os.path.join(*paths)) 

binPath = nj(cwd, "bin/Release/netcoreapp2.0/win7-x64")
publishPath = nj(cwd, "../publish/current/bin")

for _, _, files in os.walk(binPath):
    for f in files:
        fp = nj(binPath, f)
        copy(fp, publishPath)
    break;
    