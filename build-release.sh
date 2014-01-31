#!/bin/sh

function vsvers()
{
	if [ "$VS120COMNTOOLS" ]; then
		echo " /property:VisualStudioVersion=12.0"
	else
		echo ""
	fi
}

$WINDIR/Microsoft.NET/Framework/v4.0.30319/MSBuild.exe `dirname $0`/BuildRelease.msbuild `vsvers`

