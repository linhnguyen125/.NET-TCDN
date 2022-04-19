using MISA.Core.Entities;
using MISA.Core.Interfaces;
using MISA.Core.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Services
{
    public class AccountService : BaseService<Account>, IAccountService
    {
        IAccountRepository _accountRepository;
        public AccountService(IAccountRepository accountRepository) : base(accountRepository)
        {
            _accountRepository = accountRepository;
        }

        /// <summary>
        /// Validate dữ liệu cho từng đối tượng
        /// </summary>
        /// <param name="employee"></param>
        /// <returns>
        /// - true: đã validate,
        /// - false: chưa validate
        /// </returns>
        /// CreatedBy: NVLINH (11/04/2022)
        protected override bool ValidateCustom(Account account, string mode)
        {
            // EmployeeCode đã tồn tại
            if (mode == MISAFormMode.Create.ToString())
            {
                if (CheckDuplicateCode(account.account_number))
                {
                    errorData.Add("account_number", String.Format(Resources.ResourceVN.ValidateError_DuplicateAccountNumber, account.account_number));
                    return false;
                }
            }
            else if (mode == MISAFormMode.Update.ToString())

            {
                if (CheckDuplicateCode(account.account_id, account.account_number))
                {
                    errorData.Add("account_number", String.Format(Resources.ResourceVN.ValidateError_DuplicateAccountNumber, account.account_number));
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Kiểm tra mã nhân viên khi create
        /// </summary>
        /// <param name="employeeCode"></param>
        /// <returns>
        /// true - đã bị trùng, false - không bị trùng
        /// </returns>
        /// CreatedBy NVLINH (09/03/2022)
        private bool CheckDuplicateCode(string employeeCode)
        {
            return _accountRepository.CheckDuplicateCode(employeeCode);
        }

        /// <summary>
        /// Kiểm tra mã nhân viên khi update
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="employeeCode"></param>
        /// <returns>
        /// true - đã bị trùng, false - không bị trùng
        /// </returns>
        /// CreatedBy NVLINH (09/03/2022)
        private bool CheckDuplicateCode(Guid employeeId, string employeeCode)
        {
            return _accountRepository.CheckDuplicateCode(employeeId, employeeCode);
        }
    }
}
