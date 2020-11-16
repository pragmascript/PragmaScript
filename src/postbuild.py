from shutil import copy
import os
import sys
import platform


if sys.argv[1] == "-Release":
    prefixPath = "bin/Release"
elif sys.argv[1] == "-Debug":
    prefixPath = "bin/Debug"
else:
    assert False


cwd = os.getcwd()


def nj(*paths):
    return os.path.normpath(os.path.join(*paths))


if platform.system() == "Windows":
    binPath = nj(cwd, prefixPath, "net5.0/win7-x64")
elif platform.system() == "Linux":
    binPath = nj(cwd, prefixPath, "net5.0/linux-x64")

publishPath = nj(cwd, "../publish/current/bin")


for _, _, files in os.walk(binPath):
    for f in files:
        fp = nj(binPath, f)
        copy(fp, publishPath)
    break

