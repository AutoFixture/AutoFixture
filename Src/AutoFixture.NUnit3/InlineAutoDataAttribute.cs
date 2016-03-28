using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework.Interfaces;

namespace Ploeh.AutoFixture.NUnit3
{
    /// <summary>
    /// This attribute acts as a TestCaseAttribute but allow incomplete parameter values, 
    /// which will be provided by AutoFixture. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    [CLSCompliant(false)]
    public class InlineAutoDataAttribute : AutoDataAttribute
    {
        private readonly object[] _arguments;

        /// <summary>
        /// Construct a <see cref="InlineAutoDataAttribute"/>
        /// </summary>
        public InlineAutoDataAttribute(params object[] arguments)
            : this(new Fixture(), arguments)
        {
        }

        /// <summary>
        /// Construct a <see cref="InlineAutoDataAttribute"/> with an <see cref="IFixture"/> 
        /// </summary>
        protected InlineAutoDataAttribute(IFixture fixture, params object[] arguments) : base(fixture)
        {
            _arguments = arguments;
        }

        /// <summary>
        /// Get values for a collection of <see cref="IParameterInfo"/>
        /// </summary>
        protected override IEnumerable<object> GetParameterValues(IEnumerable<IParameterInfo> parameters)
        {
            var parametersWithoutValues = parameters.Skip(this._arguments.Count());

            return this._arguments.Concat(parametersWithoutValues.Select(GetValueForParameter));
        }
    } 
}