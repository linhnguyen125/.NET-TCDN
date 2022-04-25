using MISA.Core.Entities;
using MISA.Core.Exceptions;
using MISA.Core.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Services
{
    public class BaseService<MISAEntity> : IBaseService<MISAEntity> where MISAEntity : class
    {
        IBaseRepository<MISAEntity> _baseRepository;
        //Khai báo biến chứa lỗi
        public Dictionary<string, string> errorData;
        protected string _tableName = string.Empty;
        public BaseService(IBaseRepository<MISAEntity> baseRepository)
        {
            _baseRepository = baseRepository;
            errorData = new Dictionary<string, string>();
            _tableName = ToUnderscoreCase(typeof(MISAEntity).Name);
        }

        /// <summary>
        /// Nghiệp vụ thêm mới dữ liệu
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// CreatedBy: NVLINH (11/03/2022)
        public int InsertService(MISAEntity entity)
        {
            // Validate dữ liệu
            if (ValidateObject(entity, MISAFormMode.Create.ToString()))
            {
                // Thực hiện thêm mới
                var res = _baseRepository.Insert(entity);
                // Trả về dữ liệu
                return res;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Nghiệp vụ cập nhật dữ liệu
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// CreatedBy: NVLINH (11/03/2022)
        public int UpdateService(MISAEntity entity)
        {
            // Validate dữ liệu
            if (ValidateObject(entity, MISAFormMode.Update.ToString()))
            {
                // Thực hiện cập nhật
                var res = _baseRepository.Update(entity);
                // Trả về dữ liệu
                return res;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Validate dữ liệu
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="mode"></param>
        /// <returns>
        /// true: đã hợp lệ, false: chưa hợp lệ
        /// </returns>
        /// <exception cref="ValidateException"></exception>
        /// CreatedBy:  NVLINH (16/03/2022)
        private bool ValidateObject(MISAEntity entity, string mode)
        {
            //Xử lý validate chung
            var isValid = true;
            //1. Quét toàn bộ properties
            var properties = entity.GetType().GetProperties();
            var tableDisplay = GetTableName<MISAEntity>();
            var id = entity.GetType().GetProperty($"{_tableName}_id").GetValue(entity);
            var code = entity.GetType().GetProperty($"{_tableName}_code").GetValue(entity);
            //check trùng mã đầu tiên
            if (mode == MISAFormMode.Create.ToString())
            {
                if (_baseRepository.CheckDuplicateCode(code.ToString()))
                {
                    errorData.Add($"{_tableName}_code", String.Format(Resources.ResourceVN.ValidateError_DuplicateEntityCode, tableDisplay, code.ToString()));
                    isValid = false;
                }
            }
            else
            {
                if (_baseRepository.CheckDuplicateCode((Guid)id, code.ToString()))
                {
                    errorData.Add($"{_tableName}_code", String.Format(Resources.ResourceVN.ValidateError_DuplicateEntityCode, tableDisplay, code.ToString()));
                    isValid = false;
                }
            }
            if (errorData.Count > 0)
            {
                throw new ValidateException(Resources.ResourceVN.ValidateError_Invalid, errorData);
            }

            foreach (var property in properties)
            {
                //Lấy ra tên của prop
                var propName = property.Name;
                //Lấy ra giá trị của prop
                var propValue = property.GetValue(entity);
                //Lấy ra thông tin displayname của property
                var propertyNameAttribute = Attribute.GetCustomAttribute(property, typeof(MISADisplayName));
                var displayName = propName;
                if (propertyNameAttribute != null)
                {
                    displayName = (propertyNameAttribute as MISADisplayName).DisplayName;
                }
                //Kiểm tra prop có đánh dấu validate không?
                //1. Required
                var isRequired = Attribute.IsDefined(property, typeof(MISARequired));
                if (isRequired == true && (propValue == null || propValue.ToString() == string.Empty))
                {
                    errorData.Add(propName, String.Format(Resources.ResourceVN.ValidateError_BaseRequired, displayName));
                    isValid = false;
                }
                //2. Email valid
                var isValidEmail = Attribute.IsDefined(property, typeof(MISAEmailValid));
                if (isValidEmail == true && (propValue != null && propValue.ToString() != string.Empty))
                {
                    if (!IsValidEmail(propValue.ToString()))
                    {
                        errorData.Add(propName, Resources.ResourceVN.ValidateError_InvalidEmail);
                        isValid = false;
                    }
                }
                //3. MaxLength
                //Lấy ra thông tin maxLength của property
                int maxLength = 0;
                var propMaxLengthAttribute = Attribute.GetCustomAttribute(property, typeof(MISAMaxLength));
                if (propMaxLengthAttribute != null)
                {
                    maxLength = (propMaxLengthAttribute as MISAMaxLength).Length;
                }
                var isMaxLength = Attribute.IsDefined(property, typeof(MISAMaxLength));
                if (isMaxLength == true && (propValue != null && propValue.ToString() != string.Empty))
                {
                    if (propValue.ToString().Length > maxLength)
                    {
                        errorData.Add(propName, String.Format(Resources.ResourceVN.ValidateError_MaxLength, displayName, maxLength));
                        isValid = false;
                    }
                }
            }
            //2. Xử lý validate riêng cho từng đối tượng
            ValidateCustom(entity, mode);

            if (errorData.Count > 0)
            {
                throw new ValidateException(Resources.ResourceVN.ValidateError_Invalid, errorData);
            }

            return isValid;
        }

        /// <summary>
        /// Thực hiện validate đặc thù cho từng đối tượng khi thêm mới
        /// </summary>
        /// <param name="entity">Đối tượng cần validate</param>
        /// <returns>
        /// true: nếu hợp lệ, false: không hợp lệ
        /// </returns>
        /// CreatedBy: NVLINH (17/03/2022)
        protected virtual bool ValidateCustom(MISAEntity entity, string mode)
        {
            return true;
        }

        /// <summary>
        /// Kiểm tra định dạng email
        /// </summary>
        /// <param name="email"></param>
        /// <returns>
        /// true - đúng định dạng, false - không đúng định dạng
        /// </returns>
        /// CreatedBy NVLINH (09/03/2022)
        private bool IsValidEmail(string email)
        {
            var trimedEmail = email.Trim();
            if (trimedEmail.EndsWith("."))
            {
                return false;
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(trimedEmail);
                return addr.Address == trimedEmail;
            }
            catch
            {
                return false;
            }
        }

        protected string ToUnderscoreCase(string str)
        {
            return string.Concat(str.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString())).ToLower();
        }

        //protected string GetTableName()
        //{
        //    var tableAttribute = (MISATable)Attribute.GetCustomAttributes(typeof(MISATable)).FirstOrDefault();
        //    return tableAttribute.DisplayName;
        //}

        public string GetTableName<T>()
        {
            var tableAttribute = typeof(T).GetCustomAttributes(typeof(MISATable), true).FirstOrDefault() as MISATable;
            if (tableAttribute != null)
            {
                return tableAttribute.DisplayName;
            }
            return null;
        }
    }
}
