using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Entities
{
    /// <summary>
    /// Class base
    /// </summary>
    public class BaseEntity
    {
        /// <summary>
        /// Thông tin ngày tạo
        /// </summary>
        [MISAColumn]
        public DateTime? created_date { get; set; }

        /// <summary>
        /// Thông tin người tạo
        /// </summary>
        [MISAColumn]
        public string? created_by { get; set; }

        /// <summary>
        /// Ngày sửa gần nhất
        /// </summary>
        [MISAColumn]
        public DateTime? modified_date { get; set; }

        /// <summary>
        /// Người sửa gần nhất
        /// </summary>
        [MISAColumn]
        public string? modified_by { get; set; }
    }
}
