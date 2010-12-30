using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Reflection;

namespace Ploeh.AutoFixture.Idioms
{
    public class MethodContext
    {
        private readonly IFixture fixture;
        private readonly MethodInfo method;

        public MethodContext(IFixture fixture, MethodInfo methodInfo)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException("fixture");
            }
            if (methodInfo == null)
            {
                throw new ArgumentNullException("methodInfo");
            }

            this.fixture = fixture;
            this.method = methodInfo;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public IFixture Fixture
        {
            get { return this.fixture; }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public MethodInfo Method
        {
            get { return this.method; }
        }
    }
}
