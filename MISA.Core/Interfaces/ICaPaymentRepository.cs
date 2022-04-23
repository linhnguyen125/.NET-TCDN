using MISA.Core.Entities;
using MISA.Core.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Interfaces
{
    public interface ICaPaymentRepository : IBaseRepository<CaPayment>
    {
        public int InsertFull(CaPayment caPayment);

        public int UpdateFull(CaPayment caPayment);

        public Object GetPaging(FilterObject filterObject);

        public string GetNewPaymentCode();
    }
}
