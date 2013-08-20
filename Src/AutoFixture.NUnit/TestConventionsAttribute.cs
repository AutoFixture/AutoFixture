using System;

namespace Ploeh.AutoFixture.NUnit
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public abstract class TestConventionsAttribute : Attribute
    {
        private readonly IFixture _fixture;

        protected TestConventionsAttribute()
            : this(new Fixture())
        {
        }

        protected TestConventionsAttribute(IFixture fixture)
        {
            _fixture = fixture;
        }

        public IFixture Fixture 
        { 
            get { return _fixture; } 
        }
    }
}