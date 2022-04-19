using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.Core.Entities;
using MISA.Core.Interfaces;

namespace MISA.WebApi.Controllers
{
    public class CaPaymentDetailsController : MISABaseController<CaPaymentDetail>
    {
        ICaPaymentDetailRepository _caPaymentDetailRepository;
        ICaPaymentDetailService _caPaymentDetailService;
        public CaPaymentDetailsController(ICaPaymentDetailRepository caPaymentDetailRepository, ICaPaymentDetailService caPaymentDetailService) : base(caPaymentDetailRepository, caPaymentDetailService)
        {
            _caPaymentDetailRepository = caPaymentDetailRepository;
            _caPaymentDetailService = caPaymentDetailService;
        }
    }
}
