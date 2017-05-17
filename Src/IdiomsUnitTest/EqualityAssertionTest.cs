using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Ploeh.AutoFixture.Idioms;
using Ploeh.AutoFixture.Kernel;
using Ploeh.TestTypeFoundation;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
	public class EqualityAssertionTest
	{
		[Fact]
		public void SutIsIdiomaticAssertion()
		{
			// Fixture setup
			var dummyComposer = new Fixture();
			// Exercise system
			var sut = new EqualityAssertion(dummyComposer);
			// Verify outcome
			Assert.IsAssignableFrom<IdiomaticAssertion>(sut);
			// Teardown
		}

		[Fact]
		public void ComposerIsCorrect()
		{
			// Fixture setup
			var expectedComposer = new Fixture();
			var sut = new EqualityAssertion(expectedComposer);
			// Exercise system
			var result = sut.Fixture;
			// Verify outcome
			Assert.Equal(expectedComposer, result);
			// Teardown
		}

		[Fact]
		public void ComparerIsCorrect()
		{
			// Fixture setup
			Func<object, object, bool> expectedComparer = (x, y) => true;
			var sut = new EqualityAssertion(new Fixture(), expectedComparer);
			// Exercise system
			var result = sut.Comparer;
			// Verify outcome
			Assert.Equal(expectedComparer, result);
			// Teardown
		}

		[Fact]
		public void ConstructWithNullComposerThrows()
		{
			// Fixture setup
			Func<object, object, bool> dummyComparer = (x, y) => true;
			// Exercise system and verify outcome
			Assert.Throws<ArgumentNullException>(() => new EqualityAssertion(null));
			// Teardown
		}

		[Fact]
		public void ConstructWithNullComparerThrows()
		{
			// Fixture setup
			var dummyComposer = new Fixture();
			// Exercise system and verify outcome
			Assert.Throws<ArgumentNullException>(() => new EqualityAssertion(dummyComposer, null));
			// Teardown
		}

		[Fact]
		public void VerifyNullConstructorInfoThrows()
		{
			// Fixture setup
			var dummyComposer = new Fixture();
			var sut = new EqualityAssertion(dummyComposer);
			// Exercise system and verify outcome
			Assert.Throws<ArgumentNullException>(() => sut.Verify((ConstructorInfo)null));
			// Teardown
		}

		[Theory,
		InlineData(true),
		InlineData(false)]
		public void DefaultComparerShouldCallEquals(bool expected)
		{
			// Fixture setup
			var sut = new EqualityAssertion(new Fixture());
			var comparer = sut.Comparer;
			// Exercise system
			var result = comparer(new EqualityResponder(expected), new object());
			// Verify outcome
			Assert.Equal(expected, result);
			// Teardown
		}

		[Fact]
		public void VerifyConstructorInfoWhenEqualsIsProperlyImplementedDoesNotThrow()
		{
			// Fixture setup
			var sut = new EqualityAssertion(new Fixture());
			var constructor = typeof(EqualProperlyImplementedTestClass).GetConstructors().First();
			// Exercise system and verify outcome
			Assert.DoesNotThrow(() => sut.Verify(constructor));
			// Teardown
		}

		[Fact]
		public void VerifyConstructorInfoWhenEqualsIsNotProperlyImplementedThrows()
		{
			// Fixture setup
			var sut = new EqualityAssertion(new Fixture());
			var constructor = typeof(EqualNotProperlyImplementedTestClass).GetConstructors().First();
			// Exercise system and verify outcome
			Assert.Throws<EqualsOverrideException>(() => sut.Verify(constructor));
			// Teardown
		}

		[Fact]
		public void VerifyConstructorInfoWhenEqualsReturnsFalseThrows()
		{
			// Fixture setup
			var sut = new EqualityAssertion(new Fixture());
			var constructor = typeof(EqualReturnsFalse).GetConstructors().First();
			// Exercise system and verify outcome
			Assert.Throws<EqualsOverrideException>(() => sut.Verify(constructor));
			// Teardown
		}


		[Fact]
		public void VerifyConstructorInfoWhenEqualsReturnsTrueThrows()
		{
			// Fixture setup
			var sut = new EqualityAssertion(new Fixture());
			var constructor = typeof(EqualReturnsTrue).GetConstructors().First();
			// Exercise system and verify outcome
			Assert.Throws<EqualsOverrideException>(() => sut.Verify(constructor));
			// Teardown
		}

		[Fact]
		public void VerifyConstructorInfoWhenHavingEnumerableDoesNotThrow()
		{
			// Fixture setup
			var sut = new EqualityAssertion(new Fixture());
			var constructor = typeof(EqualProperlyImplementedWithArray).GetConstructors().First();
			// Exercise system and verify outcome
			Assert.DoesNotThrow(() => sut.Verify(constructor));
			// Teardown
		}

		[Fact]
		public void VerifyConstructorInfoWhenHavingEnumerableThrows()
		{
			// Fixture setup
			var sut = new EqualityAssertion(new Fixture());
			var constructor = typeof(EqualNotProperlyImplementedWithArray).GetConstructors().First();
			// Exercise system and verify outcome
			Assert.Throws<EqualsOverrideException>(() => sut.Verify(constructor));
			// Teardown
		}

		[Fact]
		public void VerifyNullPropertyInfoThrows()
		{
			// Fixture setup
			var dummyComposer = new Fixture();
			var sut = new EqualityAssertion(dummyComposer);
			// Exercise system and verify outcome
			Assert.Throws<ArgumentNullException>(() => sut.Verify((PropertyInfo)null));
			// Teardown
		}

		[Fact]
		public void VerifyPropertyInfoWhenEqualsIsProperlyImplementedDoesNotThrow()
		{
			// Fixture setup
			var sut = new EqualityAssertion(new Fixture());
			// Exercise system and verify outcome
			Assert.DoesNotThrow(() => sut.Verify(typeof(EqualProperlyImplementedTestClass).GetProperties()));
			// Teardown
		}

		[Fact]
		public void VerifyPropertyInfoWhenEqualsIsNotProperlyImplementedThrows()
		{
			// Fixture setup
			var sut = new EqualityAssertion(new Fixture());
			// Exercise system and verify outcome
			Assert.Throws<EqualsOverrideException>(() => sut.Verify(typeof(EqualNotProperlyImplementedTestClass).GetProperties()));
			// Teardown
		}

		[Fact]
		public void VerifyPropertyInfoWhenEqualsReturnsFalseDoesNotThrow()
		{
			// Fixture setup
			var sut = new EqualityAssertion(new Fixture());
			// Exercise system and verify outcome
			Assert.DoesNotThrow(() => sut.Verify(typeof(EqualReturnsFalse).GetProperties()));
			// Teardown
		}

		[Fact]
		public void VerifyPropertyInfoWhenEqualsReturnsTrueThrows()
		{
			// Fixture setup
			var sut = new EqualityAssertion(new Fixture());
			// Exercise system and verify outcome
			Assert.Throws<EqualsOverrideException>(() => sut.Verify(typeof(EqualReturnsTrue).GetProperties()));
			// Teardown
		}

		[Fact]
		public void VerifyNullFieldInfoThrows()
		{
			// Fixture setup
			var dummyComposer = new Fixture();
			var sut = new EqualityAssertion(dummyComposer);
			// Exercise system and verify outcome
			Assert.Throws<ArgumentNullException>(() => sut.Verify((FieldInfo)null));
			// Teardown
		}

		[Fact]
		public void VerifyFieldInfoWhenEqualsIsProperlyImplementedDoesNotThrow()
		{
			// Fixture setup
			var sut = new EqualityAssertion(new Fixture());
			// Exercise system and verify outcome
			Assert.DoesNotThrow(() => sut.Verify(typeof(EqualProperlyImplementedTestClass).GetFields()));
			// Teardown
		}

		[Fact]
		public void VerifyFieldInfoWhenEqualsIsNotProperlyImplementedThrows()
		{
			// Fixture setup
			var sut = new EqualityAssertion(new Fixture());
			// Exercise system and verify outcome
			Assert.Throws<EqualsOverrideException>(() => sut.Verify(typeof(EqualNotProperlyImplementedTestClass).GetFields()));
			// Teardown
		}

		[Fact]
		public void VerifyFieldInfoWhenEqualsReturnsFalseDoesNotThrow()
		{
			// Fixture setup
			var sut = new EqualityAssertion(new Fixture());
			// Exercise system and verify outcome
			Assert.DoesNotThrow(() => sut.Verify(typeof(EqualReturnsFalse).GetFields()));
			// Teardown
		}

		[Fact]
		public void VerifyFieldInfoWhenEqualsReturnsTrueThrows()
		{
			// Fixture setup
			var sut = new EqualityAssertion(new Fixture());
			// Exercise system and verify outcome
			Assert.Throws<EqualsOverrideException>(() => sut.Verify(typeof(EqualReturnsTrue).GetFields()));
			// Teardown
		}

		
		/// <summary>
		/// An class implementing Equals properly
		/// </summary>
		public class EqualProperlyImplementedTestClass : IEquatable<EqualProperlyImplementedTestClass>
		{
			private static readonly object _defaultTestB = new object();
			private readonly string _testA;
			private readonly object _testB;
			private readonly int _testC;

			public EqualProperlyImplementedTestClass(string a, object b, int c)
			{
				_testA = a;
				_testB = b ?? _defaultTestB;
				_testC = c;
			}

			public string TestA { get { return _testA; } }
			public object TestB { get { return _testB; } }
			public int TestC { get { return _testC; } }
			public string TestD { get; set; }
			public int TestE { get; set; }
			public object TestF;
			public string TestI;

			public readonly string TestJ;

			public bool Equals(EqualProperlyImplementedTestClass other)
			{
				if (other == null)
				{
					return false;
				}
				return TestA.Equals(other.TestA) &&
					TestB.Equals(other.TestB) &&
					TestC.Equals(other.TestC) &&
					TestD.Equals(other.TestD) &&
					TestE.Equals(other.TestE) &&
					TestF.Equals(other.TestF) &&
					TestI.Equals(other.TestI);
			}

			public override bool Equals(object obj)
			{
				var other = obj as EqualProperlyImplementedTestClass;
				if (other == null)
				{
					return false;
				}
				return this.Equals(other);
			}

			public override int GetHashCode()
			{
				return base.GetHashCode();
			}
		}
				
		/// <summary>
		/// An class not implementing Equals properly
		/// </summary>
		public class EqualNotProperlyImplementedTestClass : IEquatable<EqualNotProperlyImplementedTestClass>
		{
			private static readonly object _defaultTestB = new object();
			private readonly string _testA;
			private readonly object _testB;
			private readonly int _testC;

			public EqualNotProperlyImplementedTestClass(string a, object b, int c)
			{
				_testA = a;
				_testB = b ?? _defaultTestB;
				_testC = c;
			}

			public string TestA { get { return _testA; } }
			public object TestB { get { return _testB; } }
			public int TestC { get { return _testC; } }
			public string TestD { get; set; }
			public int TestE { get; set; }
			public object TestF;
			public string TestI;

			public bool Equals(EqualNotProperlyImplementedTestClass other)
			{
				if (other == null)
				{
					return false;
				}
				return TestA.Equals(other.TestA) &&
					   //omit TestB comparison
					   TestC.Equals(other.TestC) &&
					   //omit TestD
					   TestE.Equals(other.TestE) &&
					   //omit TestF comparison
					   TestI.Equals(other.TestI);
			}

			public override bool Equals(object obj)
			{
				var other = obj as EqualNotProperlyImplementedTestClass;
				if (other == null)
				{
					return false;
				}
				return this.Equals(other);
			}

			public override int GetHashCode()
			{
				return base.GetHashCode();
			}
		}

		public class EqualReturnsFalse
		{
			private readonly string _testA;
			private readonly object _testB;
			private readonly int _testC;

			public string TestA { get; set; }

			public string TestB;

			public EqualReturnsFalse(string a, object b, int c)
			{
				_testA = a;
				_testB = b;
				_testC = c;
			}

			public override bool Equals(object obj)
			{
				return false;
			}

			public override int GetHashCode()
			{
				return base.GetHashCode();
			}
		}

		public class EqualReturnsTrue
		{
			private readonly string _testA;
			private readonly object _testB;
			private readonly int _testC;

			public string TestA { get; set; }

			public string TestB;

			public EqualReturnsTrue(string a, object b, int c)
			{
				_testA = a;
				_testB = b;
				_testC = c;
			}

			public override bool Equals(object obj)
			{
				return true;
			}

			public override int GetHashCode()
			{
				return base.GetHashCode();
			}
		}

		public class EqualProperlyImplementedWithArray : IEquatable<EqualProperlyImplementedWithArray>
		{
			private readonly object[] _values;

			public EqualProperlyImplementedWithArray(IEnumerable<object> values) : this(values.ToArray())
			{
			}

			private EqualProperlyImplementedWithArray(object[] values)
			{
				_values = values;
			}

			public object[] Values { get { return _values; } }

			public bool Equals(EqualProperlyImplementedWithArray other)
			{
				return this.Values.SequenceEqual(other.Values);
			}

			public override bool Equals(object obj)
			{
				return Equals((EqualProperlyImplementedWithArray)obj);
			}

			public override int GetHashCode()
			{
				return base.GetHashCode();
			}
		}

		public class EqualNotProperlyImplementedWithArray : IEquatable<EqualNotProperlyImplementedWithArray>
		{
			private readonly object[] _values;

			public EqualNotProperlyImplementedWithArray(IEnumerable<object> values) : this(values.ToArray())
			{
			}

			private EqualNotProperlyImplementedWithArray(object[] values)
			{
				_values = values;
			}

			public object[] Values { get { return _values; } }

			public bool Equals(EqualNotProperlyImplementedWithArray other)
			{
				//Equals should use SequenceEquals when having enumerables
				return this.Values.Equals(other.Values);
			}

			public override bool Equals(object obj)
			{
				return Equals((EqualNotProperlyImplementedWithArray)obj);
			}

			public override int GetHashCode()
			{
				return base.GetHashCode();
			}
		}
	}
}
