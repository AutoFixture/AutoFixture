AutoFixture.NUnit2
=================================================

Copy packages\AutoFixture.NUnit2.{version}\lib\net40\Ploeh.AutoFixture.NUnit2.Addins.dll to the nunit-runner bin\addins folder

Known default locations
- NUnit2.6.2 -> C:\Program Files (x86)\NUnit 2.6.2\bin\addins
- R# 8 -> C:\Program Files (x86)\JetBrains\ReSharper\v8.0\Bin\addins
- TestDriven.Net -> C:\Program Files (x86)\TestDriven.NET 3\NUnit\2.6\addins

Mark Seemann explains how to use AutoDataAttribute here:
(http://blog.ploeh.dk/2010/10/08/AutoDataTheorieswithAutoFixture/)

Nunit example:

[Test, AutoData]
public void IntroductoryTest(
    int expectedNumber, MyClass sut)
{
    int result = sut.Echo(expectedNumber);
    Assert.Equal(expectedNumber, result);
}