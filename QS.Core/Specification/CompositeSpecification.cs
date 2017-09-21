using QS.Core.Specification.Contract;
using QS.Core.Specification.Implementation;

namespace QS.Core.Specification
{
    /// <summary>
    /// 复合规格的接口
    /// Base class for composite specifications
    /// </summary>
    /// <typeparam name="T">Type of entity that check this specification</typeparam>
    public abstract class CompositeSpecification<T> : Specification<T> where T : class
    {
        #region Properties

        /// <summary>
        /// Left side specification for this composite element
        /// </summary>
        public abstract ISpecification<T> LeftSideSpecification { get; }

        /// <summary>
        /// Right side specification for this composite element
        /// </summary>
        public abstract ISpecification<T> RightSideSpecification { get; }

        #endregion

    }
}
