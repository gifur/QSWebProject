using QS.Core.Specification.Implementation;
using System;
using System.Linq.Expressions;

namespace QS.Core.Specification
{
    /// <summary>
    /// A Direct Specification is a simple implementation
    /// of specification that acquire this from a lambda expression
    /// in  constructor 
    /// </summary>
    /// <typeparam name="T">Type of entity that check this specification</typeparam>
    public sealed class DirectSpecification<T> : Specification<T> where T : class
    {
        #region Members

        Expression<Func<T, bool>> _MatchingCriteria;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor for Direct Specification
        /// </summary>
        /// <param name="matchingCriteria">A Matching Criteria</param>
        public DirectSpecification(Expression<Func<T, bool>> matchingCriteria)
        {
            if (matchingCriteria == (Expression<Func<T, bool>>)null)
                throw new ArgumentNullException("matchingCriteria");

            _MatchingCriteria = matchingCriteria;
        }

        #endregion

        #region Override

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override Expression<Func<T, bool>> SatisfiedBy()
        {
            return _MatchingCriteria;
        }

        #endregion

    }
}
