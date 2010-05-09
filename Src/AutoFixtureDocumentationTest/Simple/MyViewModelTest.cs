using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Ploeh.AutoFixture;
using System.Reflection;
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
            Assert.Throws<TargetInvocationException>(() =>
                fixture.CreateAnonymous<MyViewModel>());
            // Teardown
        }

        [Fact]
        public void BuildMyViewModelWillSucceed()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system
            var mc = fixture.CreateAnonymous<MyClass>();
            var mvm = fixture.Build<MyViewModel>()
                .Do(x => x.AvailableItems.Add(mc))
                .With(x => x.SelectedItem, mc)
                .CreateAnonymous();
            // Verify outcome
            Assert.Equal<MyClass>(mc, mvm.SelectedItem);
            // Teardown
        }

        [Fact]
        public void BuildMyViewModelWithouSelectedItemWillSucceed()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system
            var mvm = fixture.Build<MyViewModel>()
                .Without(s => s.SelectedItem)
                .CreateAnonymous();
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
                .CreateAnonymous();
            // Verify outcome
            Assert.NotNull(mvm);
            // Teardown
        }

        [Fact]
        public void CreatingAnonymousCustomizedMyViewModelWillSucceed()
        {
            // Fixture setup
            var fixture = new Fixture();
            var mc = fixture.CreateAnonymous<MyClass>();
            fixture.Customize<MyViewModel>(ob => ob
                .Do(x => x.AvailableItems.Add(mc))
                .With(x => x.SelectedItem, mc));
            // Exercise system
            var mvm = fixture.CreateAnonymous<MyViewModel>();
            // Verify outcome
            Assert.Equal<MyClass>(mc, mvm.SelectedItem);
            // Teardown
        }
    }
}
