using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Entities
{
    /// <summary>
    /// Class phòng ban
    /// </summary>
    public class Department : BaseEntity
    {
        /// <summary>
        /// Thông tin ID phòng ban
        /// </summary>
        public Guid department_id { get; set; }

        /// <summary>
        /// Thông tin mã phòng ban
        /// </summary>
        [MISAMaxLength(20)]
        [MISADisplayName("Mã phòng ban")]
        public string department_code { get; set; }

        /// <summary>
        /// Thông tin tên phòng ban
        /// </summary>
        [MISAMaxLength(255)]
        [MISADisplayName("Tên phòng ban")]
        public string department_name { get; set; }
    }
}
