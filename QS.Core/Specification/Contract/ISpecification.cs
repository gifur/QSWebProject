using System;
using System.Linq.Expressions;

namespace QS.Core.Specification.Contract
{
    /// <summary>
    /// 规格接口【表示最小粒度的单位】
    /// Base contract for Specification pattern with Linq and
    /// lambda expression support
    /// Ref : http://martinfowler.com/apsupp/spec.pdf
    /// Ref : http://en.wikipedia.org/wiki/Specification_pattern
    /// Ref : http://www.codeproject.com/Articles/670115/Specification-pattern-in-Csharp
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISpecification<T> where T : class
    {
        /// <summary>
        /// 是否符合规则【指定为子类必须实现的抽象方法】
        /// Check if this specification is satisfied by a 
        /// specific expression lambda
        /// </summary>
        /// <returns></returns>
        Expression<Func<T, bool>> SatisfiedBy();
    }
}
