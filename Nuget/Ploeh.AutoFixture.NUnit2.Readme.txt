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

Known Issues

* Resharper >= 8.1 uses NUnit 2.6.3 under the covers, need to manually add binding redirect to [web|app].config

<dependentAssembly>
	<assemblyIdentity name="nunit.core.interfaces" publicKeyToken="96d09a1eb7f44a77" culture="neutral" />
	<bindingRedirect oldVersion="0.0.0.0-2.6.3.13283" newVersion="2.6.3.13283" />
</dependentAssembly>