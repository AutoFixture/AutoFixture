using System;
using System.Reflection;
using Ploeh.AutoFixture;
using Xunit;

namespace Ploeh.AutoFixtureDocumentationTest.Simple
{
    public class MyViewModelTest
    {
        public MyViewModelTest()
        {
        }

        [Fact]
        public void SetSelectedItemToAvailableItemIsLegal()
        {
            // Fixture setup
            var expectedItem = new MyClass();
            var sut = new MyViewModel();
            sut.AvailableItems.Add(expectedItem);
            // Exercise system
            sut.SelectedItem = expectedItem;
            // Verify outcome
            var result = sut.SelectedItem;
            Assert.Equal<MyClass>(expectedItem, result);
            // Teardown
        }

        [Fact]
        public void SetSelectedItemToUnavailableItemIsIllegal()
        {
            // Fixture setup
            var sut = new MyViewModel();
            sut.AvailableItems.Add(new MyClass());
            sut.AvailableItems.Add(new MyClass());
            // Exercise system and verify outcome
            Assert.Throws<ArgumentException>(() =>
                sut.SelectedItem = new MyClass());
            // Teardown
        }

        [Fact]
        public void CreateCompletelyAnonymousMyViewModelWillThrow()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system
            // Verify outcome (expected exception)
            Assert.Throws<ObjectCreationException>(() =>
                fixture.Create<MyViewModel>());
            // Teardown
        }

        [Fact]
        public void BuildMyViewModelWillSucceed()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system
            var mc = fixture.Create<MyClass>();
            var mvm = fixture.Build<MyViewModel>()
                .Do(x => x.AvailableItems.Add(mc))
                .With(x => x.SelectedItem, mc)
                .Create();
            // Verify outcome
            Assert.Equal<MyClass>(mc, mvm.SelectedItem);
            // Teardown
        }

        [Fact]
        public void BuildMyViewModelWithoutSelectedItemWillSucceed()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system
            var mvm = fixture.Build<MyViewModel>()
                .Without(s => s.SelectedItem)
                .Create();
            // Verify outcome
            Assert.NotNull(mvm);
            // Teardown
        }

        [Fact]
        public void BuildMyViewModelAndOmitAutoPropertiesWillSucceed()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system
            var mvm = fixture.Build<MyViewModel>()
                .OmitAutoProperties()
                .Create();
            // Verify outcome
            Assert.NotNull(mvm);
            // Teardown
        }

        [Fact]
        public void CreatingAnonymousCustomizedMyViewModelWillSucceed()
        {
            // Fixture setup
            var fixture = new Fixture();
            var mc = fixture.Create<MyClass>();
            fixture.Customize<MyViewModel>(ob => ob
                .Do(x => x.AvailableItems.Add(mc))
                .With(x => x.SelectedItem, mc));
            // Exercise system
            var mvm = fixture.Create<MyViewModel>();
            // Verify outcome
            Assert.Equal<MyClass>(mc, mvm.SelectedItem);
            // Teardown
        }
    }
}
