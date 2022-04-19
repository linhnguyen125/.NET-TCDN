using MISA.Core.Entities;
using MISA.Core.Interfaces;
using MISA.Core.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Services
{
    public class CaPaymentService : BaseService<CaPayment>, ICaPaymentService
    {
        ICaPaymentRepository _caPaymentRepository;
        public CaPaymentService(ICaPaymentRepository caPaymentRepository) : base(caPaymentRepository)
        {
            _caPaymentRepository = caPaymentRepository;
        }
    }
}
