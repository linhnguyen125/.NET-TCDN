using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Entities
{
    /// <summary>
    /// Enum cho formMode
    /// </summary>
    public enum MISAFormMode
    {
        Create = 1,
        Update = 2,
    }

    /// <summary>
    /// Enum cho giới tính
    /// </summary>
    public enum MISAGender
    {
        Male = 1,
        Female = 0,
        Other = 2,
    }

    public enum MISAState
    {
        InUse = 0,
        StopUse = 1,
    }

    public enum AccountObjectType
    {
        Debt = 1,
        Residual = 2,
        Hermaphrodite = 3,
        NoResidual = 4,
    }
}
