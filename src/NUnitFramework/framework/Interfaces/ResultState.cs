// Copyright (c) Charlie Poole, Rob Prouse and Contributors. MIT License - see LICENSE.txt

using System;
using System.Collections.Generic;
using System.Text;

namespace NUnit.Framework.Interfaces
{
    /// <summary>
    /// The ResultState class represents the outcome of running a test.
    /// It contains two pieces of information. The Status of the test
    /// is an enum indicating whether the test passed, failed, was
    /// skipped or was inconclusive. The Label provides a more
    /// detailed breakdown for use by client runners.
    /// </summary>
    public class ResultState : IEquatable<ResultState>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ResultState"/> class.
        /// </summary>
        /// <param name="status">The TestStatus.</param>
        public ResultState(TestStatus status) : this (status, string.Empty, FailureSite.Test)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResultState"/> class.
        /// </summary>
        /// <param name="status">The TestStatus.</param>
        /// <param name="label">The label.</param>
        public ResultState(TestStatus status, string? label) : this (status, label, FailureSite.Test)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResultState"/> class.
        /// </summary>
        /// <param name="status">The TestStatus.</param>
        /// <param name="site">The stage at which the result was produced</param>
        public ResultState(TestStatus status, FailureSite site) : this(status, string.Empty, site)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResultState"/> class.
        /// </summary>
        /// <param name="status">The TestStatus.</param>
        /// <param name="label">The label.</param>
        /// <param name="site">The stage at which the result was produced</param>
        public ResultState(TestStatus status, string? label, FailureSite site)
        {
            Status = status;
            Label = label ?? string.Empty;
            Site = site;
        }

        #endregion

        #region Predefined ResultStates

        /// <summary>
        /// The result is inconclusive
        /// </summary>
        public readonly static ResultState Inconclusive = new ResultState(TestStatus.Inconclusive);

        /// <summary>
        /// The test has been skipped.
        /// </summary>
        public readonly static ResultState Skipped = new ResultState(TestStatus.Skipped);

        /// <summary>
        /// The test has been ignored.
        /// </summary>
        public readonly static ResultState Ignored = new ResultState(TestStatus.Skipped, "Ignored");

        /// <summary>
        /// The test was skipped because it is explicit
        /// </summary>
        public readonly static ResultState Explicit = new ResultState(TestStatus.Skipped, "Explicit");

        /// <summary>
        /// The test succeeded
        /// </summary>
        public readonly static ResultState Success = new ResultState(TestStatus.Passed);

        /// <summary>
        /// The test issued a warning
        /// </summary>
        public readonly static ResultState Warning = new ResultState(TestStatus.Warning);

        /// <summary>
        /// The test failed
        /// </summary>
        public readonly static ResultState Failure = new ResultState(TestStatus.Failed);

        /// <summary>
        /// The test encountered an unexpected exception
        /// </summary>
        public readonly static ResultState Error = new ResultState(TestStatus.Failed, "Error");

        /// <summary>
        /// The test was cancelled by the user
        /// </summary>
        public readonly static ResultState Cancelled = new ResultState(TestStatus.Failed, "Cancelled");

        /// <summary>
        /// The test was not runnable.
        /// </summary>
        public readonly static ResultState NotRunnable = new ResultState(TestStatus.Failed, "Invalid");

        /// <summary>
        /// A suite failed because one or more child tests failed or had errors
        /// </summary>
        public readonly static ResultState ChildFailure = ResultState.Failure.WithSite(FailureSite.Child);

        /// <summary>
        /// A suite failed because one or more child tests had warnings
        /// </summary>
        public readonly static ResultState ChildWarning = ResultState.Warning.WithSite(FailureSite.Child);

        /// <summary>
        /// A suite is marked ignored because one or more child tests were ignored
        /// </summary>
        public readonly static ResultState ChildIgnored = ResultState.Ignored.WithSite(FailureSite.Child);

        /// <summary>
        /// A suite failed in its OneTimeSetUp
        /// </summary>
        public readonly static ResultState SetUpFailure = ResultState.Failure.WithSite(FailureSite.SetUp);

        /// <summary>
        /// A suite had an unexpected exception in its OneTimeSetUp
        /// </summary>
        public readonly static ResultState SetUpError = ResultState.Error.WithSite(FailureSite.SetUp);

        /// <summary>
        /// A suite had an unexpected exception in its OneTimeDown
        /// </summary>
        public readonly static ResultState TearDownError = ResultState.Error.WithSite(FailureSite.TearDown);

        #endregion

        #region Properties

        /// <summary>
        /// Gets the TestStatus for the test.
        /// </summary>
        /// <value>The status.</value>
        public TestStatus Status { get; }

        /// <summary>
        /// Gets the label under which this test result is
        /// categorized, or <see cref="string.Empty"/> if none.
        /// </summary>
        public string Label { get; }

        /// <summary>
        /// Gets the stage of test execution in which
        /// the failure or other result took place.
        /// </summary>
        public FailureSite Site { get; }

        /// <summary>
        /// Get a new ResultState, which is the same as the current
        /// one but with the FailureSite set to the specified value.
        /// </summary>
        /// <param name="site">The FailureSite to use</param>
        /// <returns>A new ResultState</returns>
        public ResultState WithSite(FailureSite site)
        {
            return new ResultState(this.Status, this.Label, site);
        }

        /// <summary>
        /// Test whether this ResultState has the same Status and Label
        /// as another one. In other words, the whether two are equal
        /// ignoring the Site.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Matches(ResultState other)
        {
            return Status == other.Status && Label == other.Label;
        }

        #endregion

        #region Equality

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <param name="obj">The object to compare with the current object.</param>
        public override bool Equals(object? obj)
        {
            return Equals(obj as ResultState);
        }

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(ResultState? other)
        {
            return other is not null &&
                   Status == other.Status &&
                   Label == other.Label &&
                   Site == other.Site;
        }

        /// <summary>Serves as the default hash function.</summary>
        public override int GetHashCode()
        {
            var hashCode = -665355758;
            hashCode = hashCode * -1521134295 + Status.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Label);
            hashCode = hashCode * -1521134295 + Site.GetHashCode();
            return hashCode;
        }

        #endregion

        #region Operator Overloads

        /// <summary>
        /// Overload == operator for ResultStates
        /// </summary>
        public static bool operator ==(ResultState? left, ResultState? right)
        {
            if (object.ReferenceEquals(left, null))
                return object.ReferenceEquals(right, null);

            return left.Equals(right);
        }

        /// <summary>
        /// Overload != operator for ResultStates
        /// </summary>
        public static bool operator !=(ResultState? left, ResultState? right)
        {
            return !(left == right);
        }

        #endregion

        #region ToString Override

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            var sb = new StringBuilder(Status.ToString());

            if (Label is not null && Label.Length > 0)
                sb.AppendFormat(":{0}", Label);
            if (Site != FailureSite.Test)
                sb.AppendFormat("({0})", Site.ToString());

            return sb.ToString();
        }

        #endregion
    }

    /// <summary>
    /// The FailureSite enum indicates the stage of a test
    /// in which an error or failure occurred.
    /// </summary>
    public enum FailureSite
    {
        /// <summary>
        /// Failure in the test itself
        /// </summary>
        Test,

        /// <summary>
        /// Failure in the SetUp method
        /// </summary>
        SetUp,

        /// <summary>
        /// Failure in the TearDown method
        /// </summary>
        TearDown,

        /// <summary>
        /// Failure of a parent test
        /// </summary>
        Parent,

        /// <summary>
        /// Failure of a child test
        /// </summary>
        Child
    }
}
