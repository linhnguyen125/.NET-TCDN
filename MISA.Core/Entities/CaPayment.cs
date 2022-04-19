using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Entities
{
    public class CaPayment : BaseEntity
    {
        /// <summary>
        /// khóa chính
        /// </summary>
        [MISAKey]
        [MISAColumn]
        [MISARequired]
        public Guid ca_payment_id { get; set; }

        /// <summary>
        /// mã phiếu chi
        /// </summary>
        [MISAColumn]
        [MISARequired]
        public string ca_payment_code { get; set; }

        /// <summary>
        /// địa chỉ
        /// </summary>
        [MISAColumn]
        public string? account_object_address { get; set; }

        /// <summary>
        /// mã đối tượng
        /// </summary>
        [MISAColumn]
        public string? account_object_code { get; set; }

        /// <summary>
        /// tên đối tượng
        /// </summary>
        [MISAColumn]
        public string? account_object_name { get; set; }

        /// <summary>
        /// id đối tượng
        /// </summary>
        [MISAColumn]
        public Guid account_object_id { get; set; }

        /// <summary>
        /// người nhận
        /// </summary>
        [MISAColumn]
        public string? account_object_contact_name { get; set; }

        /// <summary>
        /// lý do chi
        /// </summary>
        [MISAColumn]
        public string? journal_memo { get; set; }

        /// <summary>
        /// loại tiền
        /// </summary>
        [MISAColumn]
        public string? currency_id { get; set; }

        /// <summary>
        /// kèm theo
        /// </summary>
        [MISAColumn]
        public int? document_included { get; set; }

        /// <summary>
        /// mã nhân viên
        /// </summary>
        [MISAColumn]
        public string? employee_code { get; set; }

        /// <summary>
        /// tên nhân viên
        /// </summary>
        [MISAColumn]
        public string? employee_name { get; set; }

        /// <summary>
        /// id nhân viên
        /// </summary>
        [MISAColumn]
        public Guid employee_id { get; set; }

        /// <summary>
        /// tỉ giá
        /// </summary>
        [MISAColumn]
        public decimal? exchange_rate { get; set; }

        /// <summary>
        /// ngày hoạch toán
        /// </summary>
        [MISAColumn]
        public DateTime? posted_date { get; set; }

        /// <summary>
        /// ngày phiếu chi
        /// </summary>
        [MISAColumn]
        public DateTime? refdate { get; set; }

        /// <summary>
        /// ref đến bảng detail
        /// </summary>
        [MISAColumn]
        public string? refno_finace { get; set; }

        /// <summary>
        /// số detail
        /// </summary>
        [MISAColumn]
        public int? state { get; set; }

        /// <summary>
        /// tổng số tiền
        /// </summary>
        [MISAColumn]
        public decimal? total_amount { get; set; }
    }
}
