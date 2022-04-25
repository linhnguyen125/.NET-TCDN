using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Entities
{
    [MISATable("Nhà cung cấp")]
    public class AccountObject : BaseEntity
    {
        /// <summary>
        /// Khóa chính - id của đối tượng
        /// </summary>
        [MISAKey]
        [MISAColumn]
        public Guid? account_object_id { get; set; }

        /// <summary>
        /// Mã đối tượng
        /// </summary>
        [MISAColumn]
        [MISARequired]
        [MISADisplayName("Mã nhà cung cấp")]
        public string? account_object_code { get; set; }

        /// <summary>
        /// Tên đối tượng
        /// </summary>
        [MISAColumn]
        [MISARequired]
        [MISADisplayName("Tên nhà cung cấp")]
        public string? account_object_name { get; set; }

        /// <summary>
        /// Có phải là khách hàng không?
        /// </summary>
        [MISAColumn]
        public bool? is_customer { get; set; }

        /// <summary>
        /// Có phải là nhà cung cấp không?
        /// </summary>
        [MISAColumn]
        public bool? is_vendor { get; set; }

        /// <summary>
        /// Có phải là nhân viên không?
        /// </summary>
        [MISAColumn]
        public bool? is_employee { get; set; }

        /// <summary>
        /// Loại nhà cung cấp (0- tổ chức, 1-cá nhân)
        /// </summary>
        [MISAColumn]
        public int? account_object_kind { get; set; }

        /// <summary>
        /// giới tính(0-nữ, 1-nam, 2-khác)
        /// </summary>
        [MISAColumn]
        public MISAGender? gender { get; set; }

        /// <summary>
        /// ngày sinh
        /// </summary>
        [MISAColumn]
        public DateTime? date_of_birth { get; set; }

        /// <summary>
        /// căn cước công dân
        /// </summary>
        [MISAColumn]
        public string? identity_number { get; set; }

        /// <summary>
        /// ngày cấp căn cước công dân
        /// </summary>
        [MISAColumn]
        public DateTime? identity_date { get; set; }

        /// <summary>
        /// nơi cấp căn cước công dân
        /// </summary>
        [MISAColumn]
        public string? identity_place { get; set; }

        /// <summary>
        /// email
        /// </summary>
        [MISAColumn]
        public string? email { get; set; }

        /// <summary>
        /// số điện thoại di động
        /// </summary>
        [MISAColumn]
        public string? phone_number { get; set; }

        /// <summary>
        /// số điện thoại cố định
        /// </summary>
        [MISAColumn]
        public string? telephone_number { get; set; }

        /// <summary>
        /// thông tin tài khoản ngân hàng
        /// </summary>
        [MISAColumn]
        public string? bank_info { get; set; }

        /// <summary>
        /// địa chỉ
        /// </summary>
        [MISAColumn]
        public string? address { get; set; }

        /// <summary>
        /// địa chỉ khác
        /// </summary>
        [MISAColumn]
        public string? sub_address { get; set; }

        /// <summary>
        /// địa chỉ website
        /// </summary>
        [MISAColumn]
        public string? website { get; set; }

        /// <summary>
        /// mã số thuế
        /// </summary>
        [MISAColumn]
        public string? tax_code { get; set; }

        /// <summary>
        /// mã nhân viên (nếu account_object có chứa nhân viên)
        /// </summary>
        [MISAColumn]
        public string? employee_code { get; set; }

        /// <summary>
        /// xưng hô
        /// </summary>
        [MISAColumn]
        public int? prefix { get; set; }

        /// <summary>
        /// tên người đại diện theo pháp luật
        /// </summary>
        [MISAColumn]
        public string? legal_representative { get; set; }

        /// <summary>
        /// id tài khoản công nợ phải trả
        /// </summary>
        [MISAColumn]
        public string? pay_account { get; set; }

        /// <summary>
        /// mã đơn vị
        /// </summary>
        [MISAColumn]
        public Guid? department_id { get; set; }

        /// <summary>
        /// Tên người liên hệ
        /// </summary>
        [MISAColumn]
        public string? einvoice_contact_name { get; set; }

        /// <summary>
        /// email người liên hệ
        /// </summary>
        [MISAColumn]
        public string? contact_email { get; set; }

        /// <summary>
        /// số điện thoại người liên hệ
        /// </summary>
        [MISAColumn]
        public string? contact_mobile { get; set; }

        /// <summary>
        /// ghi chú
        /// </summary>
        [MISAColumn]
        public string? description { get; set; }

        /// <summary>
        /// id điều khoản thanh toán
        /// </summary>
        [MISAColumn]
        public string? payment_term_name { get; set; }

        /// <summary>
        /// id nhóm nhà cung cấp
        /// </summary>
        [MISAColumn]
        public string? account_object_group_id { get; set; }

        /// <summary>
        /// số ngày được nợ
        /// </summary>
        [MISAColumn]
        public int? due_time { get; set; }

        /// <summary>
        /// số nợ tối đa
        /// </summary>
        [MISAColumn]
        public Decimal? maximize_debt_amount { get; set; }

        /// <summary>
        /// quốc gia
        /// </summary>
        [MISAColumn]
        public string? country { get; set; }

        /// <summary>
        /// tên tỉnh
        /// </summary>
        [MISAColumn]
        public string? province_or_city { get; set; }

        /// <summary>
        /// tên quận/huyện/thị xã
        /// </summary>
        [MISAColumn]
        public string? district { get; set; }

        /// <summary>
        /// tên xã/phường
        /// </summary>
        [MISAColumn]
        public string? ward_or_commune { get; set; }
    }
}
