﻿using System;
using System.Reflection;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Base class for customizing parameters in methods.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = true)]
    public abstract class CustomizeAttribute : Attribute
    {
        /// <summary>
        /// Gets a customization for a parameter.
        /// </summary>
        /// <param name="parameter">The parameter for which the customization is requested.</param>
        /// <returns></returns>
        public abstract ICustomization GetCustomization(ParameterInfo parameter);
    }
}
