using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture.TUnit.UnitTest.TestTypes;
using TestTypeFoundation;

namespace AutoFixture.TUnit.UnitTest;

public class MemberAutoDataAttributeScenarioTests
{
    [Test]
    [MemberAutoData(nameof(GetSingleStringValueTestData))]
    [MemberAutoData(
        memberType: typeof(TestTypeWithMethodData),
        memberName: nameof(TestTypeWithMethodData.GetEmptyTestData))]
    [MemberAutoData(
        memberType: typeof(TestTypeWithMethodData),
        memberName: nameof(TestTypeWithMethodData.GetSingleStringValueTestData))]
    [MemberAutoData(
        memberType: typeof(TestTypeWithMethodData),
        memberName: nameof(TestTypeWithMethodData.GetStringTestsFromArgument),
        parameters: "argument")]
    public async Task SingleStringValueTest(string value)
    {
        await Assert.That(value).IsNotNull();
        await Assert.That(value).IsNotEmpty();
        await Assert.That(string.IsNullOrWhiteSpace(value)).IsFalse();
    }

    [Test]
    [MemberAutoData(nameof(GetMultipleValueTestData))]
    [MemberAutoData(
        memberType: typeof(TestTypeWithMethodData),
        memberName: nameof(TestTypeWithMethodData.GetMultipleValueTestData))]
    public async Task MultipleValueTest(string a, int b, decimal c)
    {
        await Assert.That(a).IsNotNull();
        await Assert.That(a).IsNotEmpty();
        await Assert.That(string.IsNullOrWhiteSpace(a)).IsFalse();

        await Assert.That(b != default, "Value should not be default").IsTrue();
        await Assert.That(c != default, "Value should not be default").IsTrue();
    }

    [Test]
    [MemberAutoData(nameof(GetSingleStringValueTestData))]
    [MemberAutoData(
        memberType: typeof(TestTypeWithMethodData),
        memberName: nameof(TestTypeWithMethodData.GetSingleStringValueTestData))]
    public async Task FreezesUninjectedValues(
        string a, [Frozen] string b, string c,
        PropertyHolder<string> d)
    {
        // Assert "a" ends with any possible ending from the test data
        var aSuffix = a.Split('-').Last();
        await Assert.That(new[] { "one", "two", "three" }).Contains(x => aSuffix == x);

        await Assert.That(b).IsNotNull();
        await Assert.That(b).IsNotEmpty();
        await Assert.That(string.IsNullOrWhiteSpace(b)).IsFalse();

        await Assert.That(c).IsEqualTo(b);

        await Assert.That(d).IsNotNull();
        await Assert.That(d.Property).IsEqualTo(b);
    }

    [Test]
    [MemberAutoData(nameof(GetMultipleValueTestData))]
    public async Task InjectsValues([Frozen] string a,
        [Frozen] int b,
        [Frozen] decimal c,
        PropertyHolder<string> a1,
        PropertyHolder<int> b1,
        PropertyHolder<decimal> c1)
    {
        // Assert "a" ends with any possible ending from the test data
        var aSuffix = a.Split('-').Last();
        await Assert.That(new[] { "one", "two", "three" }).Contains(x => aSuffix == x);

        await Assert.That(new[] { 22, 75, 19 }).Contains(x => x == b);
        await Assert.That(new[] { 25.7m, 228.1m, 137.09m }).Contains(x => x == c);

        await Assert.That(a1).IsNotNull();
        await Assert.That(a1.Property).IsEqualTo(a);

        await Assert.That(b1).IsNotNull();
        await Assert.That(b1.Property).IsEqualTo(b);

        await Assert.That(c1).IsNotNull();
        await Assert.That(c1.Property).IsEqualTo(c);
    }

    [Test]
    [MemberAutoData(nameof(GetStringValuesTestData))]
    [MemberAutoData(
        memberType: typeof(TestTypeWithMethodData),
        memberName: nameof(TestTypeWithMethodData.GetStringValuesTestData))]
    public async Task DoesNotAlterTestDataValuesWhenFrozen(
        [Frozen] string a, string b, string c)
    {
        var aSuffix = a.Split('-').Last();
        var bSuffix = b.Split('-').Last();
        await Assert.That(new[] { "one", "two", "three" }).Contains(x => aSuffix == x);
        await Assert.That(new[] { "uno", "dos", "tres" }).Contains(x => bSuffix == x);

        await Assert.That(b).IsNotEqualTo(a);
        await Assert.That(c).IsEqualTo(a);
    }

    [Test]
    [MemberAutoData(nameof(GetStringValuesTestData))]
    [MemberAutoData(
        memberType: typeof(TestTypeWithMethodData),
        memberName: nameof(TestTypeWithMethodData.GetStringValuesTestData))]
    public async Task LastInjectedValueIsFrozen(
        [Frozen] string a, [Frozen] string b, string c)
    {
        var aSuffix = a.Split('-').Last();
        var bSuffix = b.Split('-').Last();
        await Assert.That(new[] { "one", "two", "three" }).Contains(x => aSuffix == x);
        await Assert.That(new[] { "uno", "dos", "tres" }).Contains(x => bSuffix == x);

        await Assert.That(c).IsNotEqualTo(a);
        await Assert.That(c).IsEqualTo(b);
    }

    [Test]
    [MemberAutoData(
        memberType: typeof(TestTypeWithMethodData),
        memberName: nameof(TestTypeWithMethodData.GetTestWithComplexTypesData))]
    public async Task InjectsComplexTypes(
        [Frozen] PropertyHolder<string> a,
        PropertyHolder<string> b,
        PropertyHolder<string> c)
    {
        await Assert.That(a).IsNotNull();
        await Assert.That(b).IsNotNull();

        await Assert.That(b).IsNotSameReferenceAs(a);
        await Assert.That(c).IsSameReferenceAs(a);
    }

    public static IEnumerable<object[]> GetSingleStringValueTestData()
    {
        yield return ["test-one"];
        yield return ["test-two"];
        yield return ["test-three"];
    }

    public static IEnumerable<object[]> GetMultipleValueTestData()
    {
        yield return ["test-one", 22, 25.7m];
        yield return ["test-two", 75, 228.1m];
        yield return ["test-three", 19, 137.09m];
    }

    public static IEnumerable<object[]> GetStringValuesTestData()
    {
        yield return ["test-one", "test-uno"];
        yield return ["test-two", "test-dos"];
        yield return ["test-three", "test-tres"];
    }
}