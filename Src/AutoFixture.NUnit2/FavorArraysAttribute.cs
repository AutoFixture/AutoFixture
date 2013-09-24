﻿using System;
using System.Reflection;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.NUnit2
{
    /// <summary>
    /// An attribute that can be applied to parameters in an <see cref="AutoDataAttribute"/>-driven
    /// TestCase to indicate that the parameter value should be created using a constructor with one
    /// or more array arguments, if applicable.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class FavorArraysAttribute : CustomizeAttribute
    {
        /// <summary>
        /// Gets a customization that associates a <see cref="ArrayFavoringConstructorQuery"/> with
        /// the <see cref="Type"/> of the parameter.
        /// </summary>
        /// <param name="parameter">The parameter for which the customization is requested.</param>
        /// <returns>
        /// A customization that associates a <see cref="ArrayFavoringConstructorQuery"/> with the
        /// <see cref="Type"/> of the parameter.
        /// </returns>
        public override ICustomization GetCustomization(ParameterInfo parameter)
        {
            if (parameter == null)
                throw new ArgumentNullException("parameter");

            return new ConstructorCustomization(parameter.ParameterType, new ArrayFavoringConstructorQuery());
        }
    }
}
