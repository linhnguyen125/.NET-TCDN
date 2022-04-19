using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.Core.Entities;
using MISA.Core.Interfaces;
using MISA.Core.Interfaces.Base;

namespace MISA.WebApi.Controllers
{
    public class CaPaymentsController : MISABaseController<CaPayment>
    {
        ICaPaymentRepository _caPaymentRepository;
        ICaPaymentService _caPaymentService;
        public CaPaymentsController(ICaPaymentRepository caPaymentRepository, ICaPaymentService caPaymentService) : base(caPaymentRepository, caPaymentService)
        {
            _caPaymentRepository = caPaymentRepository;
            _caPaymentService = caPaymentService;
        }
    }
}
