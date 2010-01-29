using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;
using System.Reflection;

namespace Ploeh.AutoFixtureDocumentationTest.Simple
{
    [TestClass]
    public class MyViewModelTest
    {
        public MyViewModelTest()
        {
        }

        [TestMethod]
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
            Assert.AreEqual<MyClass>(expectedItem, result, "SelectedItem");
            // Teardown
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void SetSelectedItemToUnavailableItemIsIllegal()
        {
            // Fixture setup
            var sut = new MyViewModel();
            sut.AvailableItems.Add(new MyClass());
            sut.AvailableItems.Add(new MyClass());
            // Exercise system
            sut.SelectedItem = new MyClass();
            // Verify outcome (expected exception)
            // Teardown
        }

        [ExpectedException(typeof(TargetInvocationException))]
        [TestMethod]
        public void CreateCompletelyAnonymousMyViewModelWillThrow()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system
            var mvm = fixture.CreateAnonymous<MyViewModel>();
            // Verify outcome (expected exception)
            // Teardown
        }

        [TestMethod]
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
            Assert.AreEqual<MyClass>(mc, mvm.SelectedItem, "SelectedItem");
            // Teardown
        }

        [TestMethod]
        public void BuildMyViewModelWithouSelectedItemWillSucceed()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system
            var mvm = fixture.Build<MyViewModel>()
                .Without(s => s.SelectedItem)
                .CreateAnonymous();
            // Verify outcome
            Assert.IsNotNull(mvm, "MyViewModel");
            // Teardown
        }

        [TestMethod]
        public void BuildMyViewModelAndOmitAutoPropertiesWillSucceed()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system
            var mvm = fixture.Build<MyViewModel>()
                .OmitAutoProperties()
                .CreateAnonymous();
            // Verify outcome
            Assert.IsNotNull(mvm, "MyViewModel");
            // Teardown
        }

        [TestMethod]
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
            Assert.AreEqual<MyClass>(mc, mvm.SelectedItem, "SelectedItem");
            // Teardown
        }
    }
}
