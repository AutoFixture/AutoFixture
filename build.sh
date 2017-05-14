#!/bin/bash
if test "$OS" = "Windows_NT"
then
  # use .Net

  packages/FAKE.Core/tools/FAKE.exe $@ --fsiargs build.fsx
else
  # use mono

  mono packages/FAKE.Core/tools/FAKE.exe $@ --fsiargs -d:MONO build.fsx
fi
