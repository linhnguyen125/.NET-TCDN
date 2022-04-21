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

        /// <summary>
        /// Lấy danh sách nhân viên có phân trang
        /// </summary>
        /// <param name="filterObject"></param>
        /// <returns></returns>
        /// CreatedBy: NVLINH (09/03/2022)
        [HttpPost("filter")]
        public IActionResult GetPaging(FilterObject filterObject)
        {
            try
            {
                var res = _caPaymentRepository.GetPaging(filterObject);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
    }
}
