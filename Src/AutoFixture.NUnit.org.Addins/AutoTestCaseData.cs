using System;
using System.Collections;
using System.Collections.Specialized;
using System.Reflection;
using NUnit.Core;
using NUnit.Core.Extensibility;
using NUnit.Framework;

namespace Ploeh.AutoFixture.NUnit.org.Addins
{
    public class AutoTestCaseData : ITestCaseData
    {
        #region Constants

        private static readonly string DESCRIPTION = "_DESCRIPTION";
        //private static readonly string IGNOREREASON = "_IGNOREREASON";
        private static readonly string CATEGORIES = "_CATEGORIES";

        #endregion

        #region Instance Fields

        private readonly Exception _providerException;
        private object[] _arguments;
        private object _expectedResult;

        /// <summary>
        /// A dictionary of properties, used to add information
        /// to tests without requiring the class to change.
        /// </summary>
        private IDictionary _properties;

        #endregion

        #region Constructors

        /// <summary>
        /// Construct a non-runnable ParameterSet, specifying
        /// the provider excetpion that made it invalid.
        /// </summary>
        public AutoTestCaseData(Exception exception)
        {
            RunState = RunState.NotRunnable;
            _providerException = exception;
            IgnoreReason = exception.Message;
        }

        /// <summary>
        /// Construct an empty parameter set, which
        /// defaults to being Runnable.
        /// </summary>
        public AutoTestCaseData()
        {
            RunState = RunState.Runnable;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The RunState for this set of parameters.
        /// </summary>
        public RunState RunState { get; set; }

        ///// <summary>
        ///// The reason for not running the test case
        ///// represented by this ParameterSet
        ///// </summary>
        //public string NotRunReason
        //{
        //    get { return (string) Properties[IGNOREREASON]; }
        //}

        /// <summary>
        /// Holds any exception thrown by the parameter provider
        /// </summary>
        public Exception ProviderException
        {
            get { return _providerException; }
        }

        /// <summary>
        /// The arguments to be used in running the test,
        /// which must match the method signature.
        /// </summary>
        public object[] Arguments
        {
            get { return _arguments; }
            set
            {
                _arguments = value;

                if (OriginalArguments == null)
                    OriginalArguments = value;
            }
        }

        /// <summary>
        /// The original arguments supplied by the user,
        /// used for display purposes.
        /// </summary>
        public object[] OriginalArguments { get; private set; }

        /// <summary>
        /// The Type of any exception that is expected.
        /// </summary>
        public Type ExpectedException { get; set; }

        /// <summary>
        /// The FullName of any exception that is expected
        /// </summary>
        public string ExpectedExceptionName { get; set; }

        /// <summary>
        /// The Message of any exception that is expected
        /// </summary>
        public string ExpectedMessage { get; set; }

        /// <summary>
        ///  Gets or sets the type of match to be performed on the expected message
        /// </summary>
        public string MatchType { get; set; }

        /// <summary>
        /// The expected result of the test, which
        /// must match the method return type.
        /// </summary>
        public object Result
        {
            get { return _expectedResult; }
            set
            {
                _expectedResult = value;
                HasExpectedResult = true;
            }
        }

        /// <summary>
        /// Returns true if an expected result has been 
        /// specified for this parameter set.
        /// </summary>
        public bool HasExpectedResult { get; private set; }

        /// <summary>
        /// A description to be applied to this test case
        /// </summary>
        public string Description
        {
            get { return (string)Properties[DESCRIPTION]; }
            set
            {
                if (value != null)
                    Properties[DESCRIPTION] = value;
                else
                    Properties.Remove(DESCRIPTION);
            }
        }

        /// <summary>
        /// A name to be used for this test case in lieu
        /// of the standard generated name containing
        /// the argument list.
        /// </summary>
        public string TestName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ParameterSet"/> is ignored.
        /// </summary>
        /// <value><c>true</c> if ignored; otherwise, <c>false</c>.</value>
        public bool Ignored { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ParameterSet"/> is explicit.
        /// </summary>
        /// <value><c>true</c> if explicit; otherwise, <c>false</c>.</value>
        public bool Explicit { get; set; }

        /// <summary>
        /// Gets or sets the ignore reason.
        /// </summary>
        /// <value>The ignore reason.</value>
        public string IgnoreReason { get; set; }

        /// <summary>
        /// Gets a list of categories associated with this test.
        /// </summary>
        public IList Categories
        {
            get
            {
                if (Properties[CATEGORIES] == null)
                    Properties[CATEGORIES] = new ArrayList();

                return (IList)Properties[CATEGORIES];
            }
        }

        /// <summary>
        /// Gets the property dictionary for this test
        /// </summary>
        public IDictionary Properties
        {
            get
            {
                if (_properties == null)
                    _properties = new ListDictionary();

                return _properties;
            }
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Constructs a ParameterSet from another object, accessing properties 
        /// by reflection. The object must expose at least an Arguments property
        /// in order for the test to be runnable.
        /// </summary>
        /// <param name="source"></param>
        public static AutoTestCaseData FromAutoData(object source)
        {
            AutoTestCaseData parms = new AutoTestCaseData();

            parms.Arguments = GetParm(source, PropertyNames.Arguments) as object[];

            parms.ExpectedException = GetParm(source, PropertyNames.ExpectedException) as Type;
            if (parms.ExpectedException != null)
                parms.ExpectedExceptionName = parms.ExpectedException.FullName;
            else
                parms.ExpectedExceptionName = GetParm(source, PropertyNames.ExpectedExceptionName) as string;

            parms.ExpectedMessage = GetParm(source, PropertyNames.ExpectedMessage) as string;
            object matchEnum = GetParm(source, PropertyNames.MatchType);
            if (matchEnum != null)
                parms.MatchType = matchEnum.ToString();

            // Note: pre-2.6 versions of some attributes don't have the HasExpectedResult property
            object hasResult = GetParm(source, PropertyNames.HasExpectedResult);
            object expectedResult = GetParm(source, PropertyNames.ExpectedResult);
            if (hasResult != null && (bool)hasResult || expectedResult != null)
                parms.Result = expectedResult;

            parms.Description = GetParm(source, PropertyNames.Description) as string;

            parms.TestName = GetParm(source, PropertyNames.TestName) as string;

            object objIgnore = GetParm(source, PropertyNames.Ignored);
            if (objIgnore != null)
                parms.Ignored = (bool)objIgnore;

            parms.IgnoreReason = GetParm(source, PropertyNames.IgnoreReason) as string;

            object objExplicit = GetParm(source, PropertyNames.Explicit);
            if (objExplicit != null)
                parms.Explicit = (bool)objExplicit;

            // Some sources may also implement Properties and/or Categories
            bool gotCategories = false;
            IDictionary props = GetParm(source, PropertyNames.Properties) as IDictionary;
            if (props != null)
                foreach (string key in props.Keys)
                {
                    parms.Properties.Add(key, props[key]);
                    if (key == CATEGORIES) gotCategories = true;
                }

            // Some sources implement Categories. They may have been
            // provided as properties or they may be separate.
            if (!gotCategories)
            {
                IList categories = GetParm(source, PropertyNames.Categories) as IList;
                if (categories != null)
                    foreach (string cat in categories)
                        parms.Categories.Add(cat);
            }

            return parms;
        }

        private static object GetParm(object source, string name)
        {
            Type type = source.GetType();
            PropertyInfo prop = type.GetProperty(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty);
            if (prop != null)
                return prop.GetValue(source, null);

            FieldInfo field = type.GetField(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetField);
            if (field != null)
                return field.GetValue(source);

            return null;
        }

        #endregion
    }
}