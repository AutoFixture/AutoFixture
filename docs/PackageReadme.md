# AutoFixture

Write maintainable unit tests, faster.

AutoFixture makes it easier for developers to do Test-Driven Development by automating non-relevant Test Fixture Setup, allowing the Test Developer to focus on the essentials of each test case.

## Quick Start

```csharp
// Without AutoFixture
var customer = new Customer {
    FirstName = "John",
    LastName = "Doe",
    Email = "john@example.com",
    Age = 30
};

// With AutoFixture
var fixture = new Fixture();
var customer = fixture.Create<Customer>();
```

## Packages

### Core

| Package | Description |
|---------|-------------|
| [AutoFixture](https://www.nuget.org/packages/AutoFixture) | Core library — fixture creation, anonymous variables, test data builders |
| [AutoFixture.Idioms](https://www.nuget.org/packages/AutoFixture.Idioms) | Assertion idioms — verify guard clauses, constructor contracts, and more |
| [AutoFixture.SeedExtensions](https://www.nuget.org/packages/AutoFixture.SeedExtensions) | Seed overloads for `Create<T>()` — control initial values for reproducible tests |

### Mocking Integrations

| Package | Description |
|---------|-------------|
| [AutoFixture.AutoMoq](https://www.nuget.org/packages/AutoFixture.AutoMoq) | Auto-mocking with [Moq](https://github.com/moq/moq4) |
| [AutoFixture.AutoNSubstitute](https://www.nuget.org/packages/AutoFixture.AutoNSubstitute) | Auto-mocking with [NSubstitute](https://nsubstitute.github.io/) |
| [AutoFixture.AutoFakeItEasy](https://www.nuget.org/packages/AutoFixture.AutoFakeItEasy) | Auto-mocking with [FakeItEasy](https://fakeiteasy.github.io/) |

### Test Framework Integrations

| Package | Description |
|---------|-------------|
| [AutoFixture.Xunit3](https://www.nuget.org/packages/AutoFixture.Xunit3) | `[AutoData]` and `[InlineAutoData]` attributes for xUnit v3 |
| [AutoFixture.NUnit4](https://www.nuget.org/packages/AutoFixture.NUnit4) | `[AutoData]` and `[InlineAutoData]` attributes for NUnit 4 |

## Key Features

- **Automatic object creation** — `fixture.Create<T>()` generates realistic test data for any type
- **Customization pipeline** — freeze, build, and customize fixtures to match your test scenarios
- **Auto-mocking** — integrate with Moq, NSubstitute, or FakeItEasy to automatically create mock objects
- **Data attributes** — use `[AutoData]` and `[InlineAutoData]` to declaratively generate test parameters
- **Recursive types** — handles circular references and complex object graphs out of the box

## Example

```csharp
[Fact]
public void CustomerRepository_SavesNewCustomer()
{
    // Arrange
    var fixture = new Fixture();
    fixture.Customize<Customer>(c => c
        .With(cust => cust.IsActive, true));

    var customer = fixture.Create<Customer>();

    // Act
    var result = repository.Save(customer);

    // Assert
    Assert.True(result.IsSuccess);
}
```

## Resources

- [Cheat Sheet](https://github.com/AutoFixture/AutoFixture/wiki/Cheat-Sheet) — quick reference for common operations
- [FAQ](https://github.com/AutoFixture/AutoFixture/wiki/FAQ) — frequently asked questions
- [Who uses AutoFixture](https://github.com/AutoFixture/AutoFixture/wiki/Who-uses-AutoFixture) — testimonials and case studies
- [Pluralsight Course](https://www.pluralsight.com/courses/unit-testing-autofixture-dot-net) — video training

## License

[MIT](https://raw.githubusercontent.com/AutoFixture/AutoFixture/master/LICENCE.txt)
