using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.Idioms
{
    public class ValueObjectEqualityAssertion : IdiomaticAssertion
    {
        private readonly IFixture fixture;
        private readonly ISpecimenContext engine;
        private readonly IMethodQuery query;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueObjectEqualityAssertion"/> class.
        /// </summary>
        /// <param name="fixture">
        /// A composer which can create instances required to implement the idiomatic unit test,
        /// such as instance of given type to verify if equality works correctly.
        /// </param>
        /// <remarks>
        /// <para>
        /// <paramref name="fixture" /> will typically be a <see cref="IFixture" /> instance.
        /// </para>
        /// </remarks>
        public ValueObjectEqualityAssertion(IFixture fixture)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException("fixture");
            }

            this.fixture = fixture;
            engine = new SpecimenContext(this.Fixture);
            query = new GreedyConstructorQuery();
        }


        /// <summary>
        /// Gets the builder supplied via the constructor.
        /// </summary>
        public IFixture Fixture
        {
            get { return this.fixture; }
        }

        /// <summary>
        /// Gets the SpecimenContext built using <see cref="Fixture"/>.
        /// </summary>
        public ISpecimenContext Engine
        {
            get { return this.engine; }
        }

        /// <summary>
        /// Gets the query created within constructor.
        /// </summary>
        public IMethodQuery Query
        {
            get { return this.query; }
        }

        /// <summary>
        /// Verifies that equality for given type is implemented correctly.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Verification is being done using as follows scenarios:
        /// a. Two objects of that type are being created using every available constructor with the same parameters. For each pair 
        /// equality need to return true.
        /// b. Two objects of that type are being created using every available constructor with the same parameters. After that every possible
        /// property with setter and public field is set using the same values for both objects. For each pair equality need to return true.
        /// c. Two objects of that type are being created using every available constructor with the same parameters. After that every possible
        /// property with setter and public field is set using all the same values except of one (done in loop so that every member gets different 
        /// value while others have the same). For each pair equality need to return false.
        /// d. Two objects of that type are being created using every available constructor with different parameters. For each pair 
        /// equality need to return false.
        /// </para>
        /// <para>
        /// Equality is beinge checked by calling <see cref="IEquatable{T}.Equals(T)"/> if implemented or <see cref="Object.Equals"/> otherwise.
        /// </para>
        /// </remarks>
        /// <param name="type">Type for which equality should be checked.</param>
        public override void Verify(System.Type type)
        {
            if(type == null)
                throw new ArgumentNullException("type");

            this.VerifyCases(type, new EqualValueObjectCreator(this.Fixture, this.Query), null,
                             new ValueObjectEqualityChecker(true));

            this.VerifyCases(type, new EqualValueObjectCreator(this.Fixture, this.Query), new ValueObjectPropertyModificatorUsingTheSameValues(this.Fixture), 
                             new ValueObjectEqualityChecker(true));

            this.VerifyCases(type, new EqualValueObjectCreator(this.Fixture, this.Query), new ValueObjectFieldModificatorUsingTheSameValues(this.Fixture), 
                             new ValueObjectEqualityChecker(true));

            if (ValueObjectMemberModificator.GetProperties(type).Any())
                this.VerifyCases(type, new EqualValueObjectCreator(this.Fixture, this.Query), new ValueObjectPropertyModificatorUsingDifferentValues(this.Fixture), 
                             new ValueObjectEqualityChecker(false));

            if (ValueObjectMemberModificator.GetFields(type).Any())
                this.VerifyCases(type, new EqualValueObjectCreator(this.Fixture, this.Query), new ValueObjectFieldModificatorUsingDifferentValues(this.Fixture), 
                             new ValueObjectEqualityChecker(false));

            this.VerifyCases(type, new NotEqualValueObjectCreator(this.Fixture, this.Query), null,
                             new ValueObjectEqualityChecker(false));
        }

        private void VerifyCases(Type type, ValueObjectCreator voCreator, ValueObjectMemberModificator voMemberModificator,
                                 ValueObjectEqualityChecker voEqualityChecker)
        {
            var listOfLists = voCreator.BuildType(type, this.Engine);

            if (voMemberModificator != null)
            {
                foreach (var listOfValueObjects in listOfLists)
                {
                    voMemberModificator.ChangeMembers(listOfValueObjects, type, this.Engine);
                }
            }

            voEqualityChecker.CheckEquality(listOfLists, type);
        }
    }
}