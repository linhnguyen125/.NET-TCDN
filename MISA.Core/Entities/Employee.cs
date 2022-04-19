using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Entities
{
    /// <summary>
    /// Class nhân viên
    /// </summary>
    public class Employee : BaseEntity
    {
        #region properties
        /// <summary>
        /// Id nhân viên
        /// </summary>
        [MISAColumn]
        [MISAKey]
        public Guid employee_id { get; set; }

        /// <summary>
        /// Họ tên nhân viên
        /// </summary>
        [MISARequired]
        [MISADisplayName("Họ và tên nhân viên")]
        [MISAMaxLength(100)]
        [MISAColumn]
        public string full_name { get; set; }

        /// <summary>
        /// Mã nhân viên
        /// </summary>
        [MISARequired]
        [MISADisplayName("Mã nhân viên")]
        [MISAMaxLength(20)]
        [MISAColumn]
        public string employee_code { get; set; }

        /// <summary>
        /// Ngày sinh
        /// </summary>
        [MISAColumn]
        public DateTime? date_of_birth { get; set; }

        /// <summary>
        /// Giới tính (0-nữ, 1-nam, 2-khác)
        /// </summary>
        [MISAColumn]
        public MISAGender? gender { get; set; }

        /// <summary>
        /// Tên giới tính
        /// </summary>
        public string gender_name
        {
            get
            {
                switch (gender)
                {
                    case MISAGender.Female:
                        return "Nữ";
                    case MISAGender.Male:
                        return "Nam";
                    default:
                        return "Khác";
                }
            }
        }

        /// <summary>
        /// Tên chức danh
        /// </summary>
        [MISADisplayName("Vị trí")]
        [MISAMaxLength(50)]
        [MISAColumn]
        public string? position_name { get; set; }

        /// <summary>
        /// CMND, CCCD, Passport
        /// </summary>
        [MISADisplayName("Số CMND")]
        [MISAMaxLength(50)]
        [MISAColumn]
        public string? identity_number { get; set; }

        /// <summary>
        /// Ngày cấp CMND, CCCD, Passport
        /// </summary>
        [MISAColumn]
        public DateTime? identity_date { get; set; }

        /// <summary>
        /// Nơi cấp CMND, CCCD, Passport
        /// </summary>
        [MISADisplayName("Nơi cấp")]
        [MISAMaxLength(255)]
        [MISAColumn]
        public string? identity_place { get; set; }

        /// <summary>
        /// Email nhân viên
        /// </summary>
        [MISAEmailValid]
        [MISADisplayName("Email")]
        [MISAMaxLength(100)]
        [MISAColumn]
        public string? email { get; set; }

        /// <summary>
        /// Số điện thoại di động
        /// </summary>
        [MISADisplayName("Số điện thoại")]
        [MISAMaxLength(50)]
        [MISAColumn]
        public string? phone_number { get; set; }

        /// <summary>
        /// Mã số thuế cá nhân
        /// </summary>
        [MISADisplayName("Mã số thuế")]
        [MISAMaxLength(25)]
        [MISAColumn]
        public string? tax_code { get; set; }

        /// <summary>
        /// Mức lương cơ bản
        /// </summary>
        [MISADisplayName("Lương")]
        [MISAMaxLength(18)]
        [MISAColumn]
        public Decimal? salary { get; set; }

        /// <summary>
        /// Ngày gia nhập công ty
        /// </summary>
        [MISAColumn]
        public DateTime? join_date { get; set; }

        /// <summary>
        /// Địa chỉ nhân viên
        /// </summary>
        [MISADisplayName("Địa chỉ")]
        [MISAMaxLength(255)]
        [MISAColumn]
        public string? address { get; set; }

        /// <summary>
        /// Số điện thoại cố định
        /// </summary>
        [MISADisplayName("ĐT cố định")]
        [MISAMaxLength(50)]
        [MISAColumn]
        public string? land_line { get; set; }

        /// <summary>
        /// Tài khoản ngân hàng
        /// </summary>
        [MISADisplayName("TK ngân hàng")]
        [MISAMaxLength(50)]
        [MISAColumn]
        public string? bank_account { get; set; }

        /// <summary>
        /// Tên ngân hàng
        /// </summary>
        [MISADisplayName("Tên ngân hàng")]
        [MISAMaxLength(255)]
        [MISAColumn]
        public string? bank_name { get; set; }

        /// <summary>
        /// Chi nhánh ngân hàng
        /// </summary>
        [MISADisplayName("Chi nhánh ngân hàng")]
        [MISAMaxLength(255)]
        [MISAColumn]
        public string? bank_branch { get; set; }

        /// <summary>
        /// Tình trạng làm việc (0-đã nghỉ việc, 1-đang làm việc, 2-đang thử việc, 3-bị đình chỉ)
        /// </summary>
        public int? work_status { get; set; }

        /// <summary>
        /// Mã phòng ban
        /// </summary>
        [MISARequired]
        [MISAColumn]
        public Guid department_id { get; set; }

        /// <summary>
        /// Tên phòng ban
        /// </summary>
        [MISAColumn]
        public string? department_name { get; set; }
        #endregion
    }
}
