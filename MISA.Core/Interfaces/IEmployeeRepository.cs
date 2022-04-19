using MISA.Core.Entities;
using MISA.Core.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Interfaces
{
    public interface IEmployeeRepository : IBaseRepository<Employee>
    {
        /// <summary>
        /// Lấy danh sách nhân viên có phân trang
        /// </summary>
        /// <returns></returns>
        /// CreatedBy: NVLINH (09/03/2022)
        public Object GetPaging(int pageSize, int pageNumber, string? txtSearch);

        /// <summary>
        /// Lấy mã nhân viên mới
        /// </summary>
        /// <returns></returns>
        public string GetNewEmployeeCode();
    }
}
