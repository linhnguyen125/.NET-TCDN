using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.Core.Entities;
using MISA.Core.Interfaces;

namespace MISA.WebApi.Controllers
{
    public class DepartmentsController : MISABaseController<Department>
    {
        IDepartmentRepository _departmentRepository;
        IDepartmentService _departmentService;
        public DepartmentsController(IDepartmentRepository departmentRepository, IDepartmentService departmentService) : base(departmentRepository, departmentService)
        {
            this._departmentRepository = departmentRepository;
            this._departmentService = departmentService;
        }
    }
}
