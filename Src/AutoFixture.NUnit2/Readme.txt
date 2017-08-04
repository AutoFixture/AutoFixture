AutoFixture.NUnit2
======================

Mark Seemann explains how to use AutoDataAttribute here:
(http://blog.ploeh.dk/2010/10/08/AutoDataTheorieswithAutoFixture/)

Nunit example:

[Test, AutoData]
public void IntroductoryTest(int expectedNumber, MyClass sut)
{
    int result = sut.Echo(expectedNumber);
    Assert.Equal(expectedNumber, result);
}


NUnit 2.6.4 support
======================
Recent versions of ReSharper and "NUnit Test Adapter 2" use NUnit 2.6.4 under the hood. 
To support NUnit 2.6.4 you need to add the binding redirects to your [web|app].config file:

<?xml version="1.0" encoding="utf-8" ?>
<configuration>
  <runtime>
    <assemblyBinding xmlns="urn:schemas-microsoft-com:asm.v1">
      <dependentAssembly>
        <assemblyIdentity name="nunit.core.interfaces" publicKeyToken="96d09a1eb7f44a77" culture="neutral" />
        <bindingRedirect oldVersion="0.0.0.0-2.6.4.14350" newVersion="2.6.4.14350" />
      </dependentAssembly>
      <dependentAssembly>
        <assemblyIdentity name="nunit.core" publicKeyToken="96d09a1eb7f44a77" culture="neutral" />
        <bindingRedirect oldVersion="0.0.0.0-2.6.4.14350" newVersion="2.6.4.14350" />
      </dependentAssembly>
    </assemblyBinding>
  </runtime>
</configuration>

See more details here: https://github.com/AutoFixture/AutoFixture/issues/488
