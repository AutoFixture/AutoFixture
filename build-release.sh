#!/bin/sh

function abspath()
{
	# Make an absolute path from a relative path, and convert it from Linux to Windows format
	# Lifted from
	# http://stackoverflow.com/a/3915420/126014
	# http://stackoverflow.com/a/13701495/126014
	echo $(cd $(dirname $1); pwd)/$(basename $1) | sed 's/^\///' | sed 's/\//\\/g' | sed 's/^./\0:/'
}

function vsvers()
{
	if [ "$VS140COMNTOOLS" ]; then
		echo " /property:VisualStudioVersion=14.0"
	elif [ "$VS120COMNTOOLS" ]; then
		echo " /property:VisualStudioVersion=12.0"
	else
		echo ""
	fi
}

if [ "$1" != "" ]; then
	$WINDIR/Microsoft.NET/Framework/v4.0.30319/MSBuild.exe /property:AssemblyOriginatorKeyFile=`abspath $1` `dirname $0`/BuildRelease.msbuild `vsvers`
else
	$WINDIR/Microsoft.NET/Framework/v4.0.30319/MSBuild.exe `dirname $0`/BuildRelease.msbuild `vsvers`
fi