using MISA.Core.Entities;
using MISA.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Services
{
    public class CaPaymentDetailService : BaseService<CaPaymentDetail>, ICaPaymentDetailService
    {
        ICaPaymentDetailRepository _caPaymentDetailRepository;
        public CaPaymentDetailService(ICaPaymentDetailRepository caPaymentDetailRepository) : base(caPaymentDetailRepository)
        {
            _caPaymentDetailRepository = caPaymentDetailRepository;
        }
    }
}
