using MISA.Core.Entities;
using MISA.Core.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Interfaces
{
    public interface ICaPaymentDetailRepository : IBaseRepository<CaPaymentDetail>
    {
        public IEnumerable<CaPaymentDetail> GetByRefid(Guid refid);
    }
}
