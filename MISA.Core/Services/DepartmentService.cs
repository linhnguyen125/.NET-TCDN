using MISA.Core.Entities;
using MISA.Core.Exceptions;
using MISA.Core.Interfaces;
using MISA.Core.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Services
{
    public class DepartmentService : BaseService<Department>, IDepartmentService
    {
        IDepartmentRepository _departmentRepository;
        public DepartmentService(IDepartmentRepository departmentRepository) : base(departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        /// <summary>
        /// Validate dữ liệu khi cập nhật phòng ban
        /// </summary>
        /// <param name="departmentId"></param>
        /// <param name="department"></param>
        /// <returns>
        /// - true: đã validate,
        /// - false: chưa validate
        /// </returns>
        /// CreatedBy: NVLINH (11/03/2022)
        protected override bool ValidateCustom(Department department, string mode)
        {
            // EmployeeCode đã tồn tại
            if (mode == MISAFormMode.Create.ToString())
            {
                if (CheckDuplicateCode(department.department_code))
                {
                    errorData.Add("DepartmentCode", String.Format(Resources.ResourceVN.ValidateError_DuplicateDepartmentCode, department.department_code));
                    return false;
                }
            }
            else if (mode == MISAFormMode.Update.ToString())
            {
                if (CheckDuplicateCode(department.department_id, department.department_code))
                {
                    errorData.Add("DepartmentCode", String.Format(Resources.ResourceVN.ValidateError_DuplicateDepartmentCode, department.department_code));
                    return false;
                }
            }
            return true;
        }

        #region Validate Department
        /// <summary>
        /// Kiểm tra mã phòng ban khi create
        /// </summary>
        /// <param name="departmentCode"></param>
        /// <returns>
        /// true - đã bị trùng, false - không bị trùng
        /// </returns>
        /// CreatedBy NVLINH (09/03/2022)
        private bool CheckDuplicateCode(string departmentCode)
        {
            return _departmentRepository.CheckDuplicateCode(departmentCode);
        }

        /// <summary>
        /// Kiểm tra mã phòng ban khi update
        /// </summary>
        /// <param name="departmentId"></param>
        /// <param name="departmentCode"></param>
        /// <returns>
        /// true - đã bị trùng, false - không bị trùng
        /// </returns>
        /// CreatedBy NVLINH (09/03/2022)
        private bool CheckDuplicateCode(Guid departmentId, string departmentCode)
        {
            return _departmentRepository.CheckDuplicateCode(departmentId, departmentCode);
        }
        #endregion
    }
}
