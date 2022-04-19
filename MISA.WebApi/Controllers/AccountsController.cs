using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.Core.Entities;
using MISA.Core.Interfaces;

namespace MISA.WebApi.Controllers
{
    public class AccountsController : MISABaseController<Account>
    {
        IAccountRepository _accountRepository;
        IAccountService _accountService;
        public AccountsController(IAccountRepository accountRepository, IAccountService accountService) : base(accountRepository, accountService)
        {
            _accountRepository = accountRepository;
            _accountService = accountService;
        }

        /// <summary>
        /// Lấy danh sách nhân viên có phân trang
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="txtSearch"></param>
        /// <returns></returns>
        /// CreatedBy: NVLINH (09/03/2022)
        [HttpGet("filter")]
        public IActionResult GetPaging([FromQuery] int pageSize, int pageNumber, string? txtSearch)
        {
            try
            {
                var res = _accountRepository.GetPaging(pageSize, pageNumber, txtSearch);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
    }
}
