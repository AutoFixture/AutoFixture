﻿using System;
using System.Linq;
using NUnit.Framework;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.NUnit.UnitTest.TestCases
{
    public class FavorArraysAttributeTests
    {
        [Test]
        public void SutIsAttribute()
        {
            // Fixture setup
            // Exercise system
            var sut = new FavorArraysAttribute();
            // Verify outcome
            Assert.IsInstanceOf<CustomizeAttribute>(sut);
            // Teardown
        }

        [Test]
        public void GetCustomizationFromNullParameterThrows()
        {
            // Fixture setup
            var sut = new FavorArraysAttribute();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.GetCustomization(null));
            // Teardown
        }

        [Test]
        public void GetCustomizationReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new FavorArraysAttribute();
            var parameter = typeof(TypeWithMembers).GetMethod("DoSomething").GetParameters().Single();
            // Exercise system
            var result = sut.GetCustomization(parameter);
            // Verify outcome
            Assert.IsInstanceOf<ConstructorCustomization>(result);
            var invoker = (ConstructorCustomization) result;
            Assert.That(parameter.ParameterType, Is.EqualTo(invoker.TargetType));
            Assert.IsInstanceOf<ArrayFavoringConstructorQuery>(invoker.Query);
            // Teardown
        }
    }
}
