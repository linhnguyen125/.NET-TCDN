using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.Core.Entities;
using MISA.Core.Interfaces;
using MISA.Core.Interfaces.Base;

namespace MISA.WebApi.Controllers
{
    public class AccountObjectsController : MISABaseController<AccountObject>
    {
        IAccountObjectRepository _accountObjectRepository;
        IAccountObjectService _accountObjectService;
        public AccountObjectsController(IAccountObjectRepository accountObjectRepository, IAccountObjectService accountObjectService) : base(accountObjectRepository, accountObjectService)
        {
            _accountObjectRepository = accountObjectRepository;
            _accountObjectService = accountObjectService;
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
                var res = _accountObjectRepository.GetPaging(filterObject);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Lấy account object theo mã
        /// </summary>
        /// <param name="account_object_code"></param>
        /// <returns></returns>
        [HttpGet("getByCode")]
        public IActionResult GetByCode(string account_object_code)
        {
            try
            {
                var res = _accountObjectRepository.GetByCode(account_object_code);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("newAccountObjectCode")]
        public IActionResult GetNewCode()
        {
            try
            {
                var res = _accountObjectRepository.GetNewtCode();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
    }
}
