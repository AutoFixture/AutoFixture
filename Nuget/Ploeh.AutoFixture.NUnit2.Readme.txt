AutoFixture.NUnit2
=================================================

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