using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.Core.Entities;
using MISA.Core.Exceptions;
using MISA.Core.Interfaces;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;

namespace MISA.WebApi.Controllers
{
    public class EmployeesController : MISABaseController<Employee>
    {
        IEmployeeRepository _employeeRepository;
        IEmployeeService _employeeService;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public EmployeesController(IEmployeeRepository employeeRepository, IEmployeeService employeeService, IWebHostEnvironment hostingEnvironment) : base(employeeRepository, employeeService)
        {
            this._employeeRepository = employeeRepository;
            this._employeeService = employeeService;
            _hostingEnvironment = hostingEnvironment;
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
                var res = _employeeRepository.GetPaging(pageSize, pageNumber, txtSearch);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Lấy mã nhân viên mới
        /// </summary>
        /// <returns></returns>
        /// CreatedBy: NVLINH (10/03/2022)
        [HttpGet("newEmployeeCode")]
        public IActionResult GetNewEmployeeCode()
        {
            try
            {
                var res = _employeeRepository.GetNewEmployeeCode();
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
        public IActionResult Export(List<TableExport> tableExports)
        {
            var stream = _employeeService.Export(tableExports);
            return File(stream, "application/octet-stream");
        }

        /// <summary>
        /// Trả về lỗi Validate
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        /// CreatedBy: NVLINH (09/03/2022)
        private IActionResult HandleValidateException(EmployeeValidateException ex)
        {
            var notify = new NotifyService();
            notify.DevMsg = ex.Message;
            notify.UserMsg = MISA.Core.Resources.ResourceVN.Error_Exception;
            notify.Data = ex.Data;
            notify.StatusCode = 400;
            return BadRequest(notify);
        }
    }
}
