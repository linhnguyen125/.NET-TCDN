using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Entities
{
    /// <summary>
    /// Class thông báo
    /// </summary>
    /// CreatedBy: Nguyễn Văn Linh (09/03/2022)
    public class NotifyService
    {
        public NotifyService()
        {

        }
        public NotifyService(string devMsg, string userMsg, object? data, int? statusCode)
        {
            this.DevMsg = devMsg;
            this.UserMsg = userMsg;
            this.Data = data;
            this.StatusCode = statusCode;
        }

        /// <summary>
        /// Thông báo cho dev
        /// </summary>
        public string DevMsg { get; set; }

        /// <summary>
        /// Thông báo cho người dùng
        /// </summary>
        public string UserMsg { get; set; }

        /// <summary>
        /// Dữ liệu kèm theo thông báo
        /// </summary>
        public object? Data { get; set; }

        /// <summary>
        /// Mã thông báo
        /// </summary>
        public int? StatusCode { get; set; }

        public Object Success(string devMsg = "Thành công", string userMsg = "Thành công", object? data = null, int? statusCode = 200)
        {
            return new NotifyService(devMsg, userMsg, data, statusCode);
        }
    }
}
