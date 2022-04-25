using MISA.Core.Entities;
using MISA.Core.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Interfaces
{
    public interface IAccountObjectRepository : IBaseRepository<AccountObject>
    {
        public Object GetPaging(FilterObject filterObject);

        public Object GetByCode(string account_object_code);

        public string GetNewtCode();
    }
}
