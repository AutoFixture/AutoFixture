using System;
using AutoFixture;
using Xunit;

namespace AutoFixtureDocumentationTest.Simple
{
    public class MyViewModelTest
    {
        public MyViewModelTest()
        {
        }

        [Fact]
        public void SetSelectedItemToAvailableItemIsLegal()
        {
            // Arrange
            var expectedItem = new MyClass();
            var sut = new MyViewModel();
            sut.AvailableItems.Add(expectedItem);
            // Act
            sut.SelectedItem = expectedItem;
            // Assert
            var result = sut.SelectedItem;
            Assert.Equal<MyClass>(expectedItem, result);
        }

        [Fact]
        public void SetSelectedItemToUnavailableItemIsIllegal()
        {
            // Arrange
            var sut = new MyViewModel();
            sut.AvailableItems.Add(new MyClass());
            sut.AvailableItems.Add(new MyClass());
            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                sut.SelectedItem = new MyClass());
        }

        [Fact]
        public void CreateCompletelyAnonymousMyViewModelWillThrow()
        {
            // Arrange
            var fixture = new Fixture();
            // Act
            // Assert (expected exception)
            Assert.ThrowsAny<ObjectCreationException>(() =>
                fixture.Create<MyViewModel>());
        }

        [Fact]
        public void BuildMyViewModelWillSucceed()
        {
            // Arrange
            var fixture = new Fixture();
            // Act
            var mc = fixture.Create<MyClass>();
            var mvm = fixture.Build<MyViewModel>()
                .Do(x => x.AvailableItems.Add(mc))
                .With(x => x.SelectedItem, mc)
                .Create();
            // Assert
            Assert.Equal<MyClass>(mc, mvm.SelectedItem);
        }

        [Fact]
        public void BuildMyViewModelWithoutSelectedItemWillSucceed()
        {
            // Arrange
            var fixture = new Fixture();
            // Act
            var mvm = fixture.Build<MyViewModel>()
                .Without(s => s.SelectedItem)
                .Create();
            // Assert
            Assert.NotNull(mvm);
        }

        [Fact]
        public void BuildMyViewModelAndOmitAutoPropertiesWillSucceed()
        {
            // Arrange
            var fixture = new Fixture();
            // Act
            var mvm = fixture.Build<MyViewModel>()
                .OmitAutoProperties()
                .Create();
            // Assert
            Assert.NotNull(mvm);
        }

        [Fact]
        public void CreatingAnonymousCustomizedMyViewModelWillSucceed()
        {
            // Arrange
            var fixture = new Fixture();
            var mc = fixture.Create<MyClass>();
            fixture.Customize<MyViewModel>(ob => ob
                .Do(x => x.AvailableItems.Add(mc))
                .With(x => x.SelectedItem, mc));
            // Act
            var mvm = fixture.Create<MyViewModel>();
            // Assert
            Assert.Equal<MyClass>(mc, mvm.SelectedItem);
        }
    }
}
