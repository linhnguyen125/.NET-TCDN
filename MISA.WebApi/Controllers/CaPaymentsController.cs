using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.Core.Entities;
using MISA.Core.Exceptions;
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
        /// Hàm insert phiếu chi
        /// </summary>
        /// <param name="payment"></param>
        /// <returns></returns>
        /// CreatedBy: NVLINH 23/04/2022
        [HttpPost("full")]
        public IActionResult InsertFull(CaPayment payment)
        {
            try
            {
                var res = _caPaymentRepository.InsertFull(payment);
                if (res > 0)
                {
                    var notify = new NotifyService();
                    return StatusCode(201,
                        notify.Success
                        (
                            devMsg: MISA.Core.Resources.ResourceVN.Success_Created,
                            userMsg: MISA.Core.Resources.ResourceVN.Success_Created,
                            data: payment,
                            statusCode: 201
                        ));
                }
                return Ok(res);
            }
            catch (ValidateException ex)
            {
                return HandleValidateException(ex);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Hàm insert phiếu chi
        /// </summary>
        /// <param name="payment"></param>
        /// <returns></returns>
        /// CreatedBy: NVLINH 23/04/2022
        [HttpPut("full")]
        public IActionResult UpdateFull(CaPayment payment)
        {
            try
            {
                var res = _caPaymentRepository.UpdateFull(payment);
                if (res > 0)
                {
                    var notify = new NotifyService();
                    return Ok(notify.Success(
                            devMsg: MISA.Core.Resources.ResourceVN.Success_Updated,
                            userMsg: MISA.Core.Resources.ResourceVN.Success_Updated,
                            data: payment,
                            statusCode: 200
                        )
                    );
                }
                return Ok();
            }
            catch (ValidateException ex)
            {
                return HandleValidateException(ex);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
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

        /// <summary>
        /// Hàm lấy mã phiếu chi mới
        /// </summary>
        /// <returns></returns>
        /// CreatedBy: NVLINH (18/04/2022)
        [HttpGet("newPaymentCode")]
        public IActionResult GetNewPaymentCode()
        {
            try
            {
                var res = _caPaymentRepository.GetNewPaymentCode();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Export dữ liệu ra file excel
        /// </summary>
        /// <param name="tableExports"></param>
        /// <returns></returns>
        /// CreatedBy: NVLINH (018/03/2022)
        [HttpPost("export")]
        public IActionResult Export(FilterObject filterObject)
        {
            var stream = _caPaymentService.Export(filterObject);
            return File(stream, "application/octet-stream");
        }
    }
}
