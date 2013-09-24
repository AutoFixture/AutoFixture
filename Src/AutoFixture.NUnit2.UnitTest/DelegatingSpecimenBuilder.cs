﻿using System;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.NUnit2.UnitTest
{
    internal class DelegatingSpecimenBuilder : ISpecimenBuilder
    {
        public DelegatingSpecimenBuilder()
        {
            this.OnCreate = (r, c) => new object();
        }

        public object Create(object request, ISpecimenContext context)
        {
            return this.OnCreate(request, context);
        }

        internal Func<object, ISpecimenContext, object> OnCreate { get; set; }
    }
}
