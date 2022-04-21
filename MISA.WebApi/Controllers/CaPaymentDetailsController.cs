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

        /// <summary>
        /// Lấy chi tiết phiếu chi theo refid
        /// </summary>
        /// <param name="refid"></param>
        /// <returns></returns>
        [HttpGet("getByRefid")]
        public IActionResult GetByRefid(Guid refid)
        {
            try
            {
                var res = _caPaymentDetailRepository.GetByRefid(refid);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
    }
}
