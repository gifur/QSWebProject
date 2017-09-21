using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QS.Common
{
    /// <summary>
    /// Base class for entities
    /// </summary>
    public abstract class Entity
    {
        #region Members

        int? _requestedHashCode;

        #endregion

        #region Properties

        #endregion

        #region Public Methods


        #endregion

        #region Overrides Methods


        public static bool operator ==(Entity left, Entity right)
        {
            if (Object.Equals(left, null))
                return (Object.Equals(right, null)) ? true : false;
            else
                return left.Equals(right);
        }

        public static bool operator !=(Entity left, Entity right)
        {
            return !(left == right);
        }

        #endregion
    }
}
