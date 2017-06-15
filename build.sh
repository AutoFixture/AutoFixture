#!/bin/bash
if test "$OS" = "Windows_NT"
then
  # use .Net

  tools/FAKE.Core/tools/FAKE.exe $@ --fsiargs build.fsx
else
  # use mono

  mono tools/FAKE.Core/tools/FAKE.exe $@ --fsiargs -d:MONO build.fsx
fi
