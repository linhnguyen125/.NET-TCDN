using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Entities
{
    public class Account : BaseEntity
    {
        /// <summary>
        /// id tài khoản
        /// </summary>
        /// [MISAColumn]
        [MISAKey]
        [MISAColumn]
        public Guid account_id { get; set; }

        /// <summary>
        /// số tài khoản
        /// </summary>
        [MISAColumn]
        [MISARequired]
        [MISADisplayName("Số tài khoản")]
        public string account_number { get; set; }

        /// <summary>
        /// tên tài khoản
        /// </summary>
        [MISAColumn]
        [MISARequired]
        [MISADisplayName("Tên tài khoản")]
        public string account_name { get; set; }

        /// <summary>
        /// tài khoản tổng hợp
        /// </summary>
        [MISAColumn]
        [MISADisplayName("Tài khoản tổng hợp")]
        public Guid? parent_id { get; set; }

        /// <summary>
        /// tên tiếng anh
        /// </summary>
        [MISAColumn]
        [MISADisplayName("Tên tiếng anh")]
        public string? account_name_english { get; set; }

        /// <summary>
        /// tính chất
        /// </summary>
        [MISAColumn]
        [MISARequired]
        [MISADisplayName("Tính chất")]
        public AccountObjectType? account_object_type { get; set; }

        public string account_object_type_name
        {
            get
            {
                switch (account_object_type)
                {
                    case AccountObjectType.Debt:
                        return "Dư nợ";
                    case AccountObjectType.Residual:
                        return "Dư có";
                    case AccountObjectType.Hermaphrodite:
                        return "Lưỡng tính";
                    default:
                        return "Không có số dư";
                }
            }
        }

        /// <summary>
        /// có hoạch toán ngoại tệ
        /// </summary>
        [MISAColumn]
        public bool? is_postable_in_foreign_currency { get; set; }

        /// <summary>
        /// diễn giải
        /// </summary>
        [MISAColumn]
        [MISADisplayName("Diễn giải")]
        public string? description { get; set; }

        /// <summary>
        /// Trạng thái
        /// </summary>
        [MISAColumn]
        public MISAState? state { get; set; }

        public string state_name
        {
            get
            {
                switch (state)
                {
                    case MISAState.InUse:
                        return "Đang sử dụng";
                    case MISAState.StopUse:
                        return "Ngừng sử dụng";
                    default:
                        return "Đang sử dụng";
                }
            }
        }

        /// <summary>
        /// theo dõi theo đối tượng
        /// </summary>
        [MISAColumn]
        public bool? detail_by_account_object { get; set; }

        [MISAColumn]
        public int? detail_by_account_object_kind { get; set; }

        /// <summary>
        /// theo dõi theo tài khoản ngân hàng
        /// </summary>
        [MISAColumn]
        public bool? detail_by_bank_account { get; set; }

        /// <summary>
        /// theo dõi theo hợp đồng bán
        /// </summary>
        [MISAColumn]
        public bool? detail_by_contract { get; set; }

        [MISAColumn]
        public int? detail_by_contract_kind { get; set; }

        /// <summary>
        /// theo dõi theo đơn vị
        /// </summary>
        [MISAColumn]
        public bool? detail_by_department { get; set; }

        [MISAColumn]
        public int? detail_by_department_kind { get; set; }

        /// <summary>
        /// theo dõi theo khoản mục chi phí
        /// </summary>
        [MISAColumn]
        public bool? detail_by_expense_item { get; set; }

        [MISAColumn]
        public int? detail_by_expense_item_kind { get; set; }

        /// <summary>
        /// theo dõi theo đối tượng tập hợp chi phí
        /// </summary>
        [MISAColumn]
        public bool? detail_by_job { get; set; }

        [MISAColumn]
        public int? detail_by_job_kind { get; set; }

        /// <summary>
        /// theo dõi theo mã thống kê
        /// </summary>
        [MISAColumn]
        public bool? detail_by_list_item { get; set; }

        [MISAColumn]
        public int? detail_by_list_item_kind { get; set; }

        /// <summary>
        /// theo dõi theo đơn đặt hàng
        /// </summary>
        [MISAColumn]
        public bool? detail_by_order { get; set; }

        [MISAColumn]
        public int? detail_by_order_kind { get; set; }

        /// <summary>
        /// theo dõi theo công trình
        /// </summary>
        [MISAColumn]
        public bool? detail_by_project_work { get; set; }

        [MISAColumn]
        public int? detail_by_project_work_kind { get; set; }

        /// <summary>
        /// theo dõi theo hợp đồng mua
        /// </summary>
        [MISAColumn]
        public bool? detail_by_pu_contract { get; set; }

        [MISAColumn]
        public int? detail_by_pu_contract_kind { get; set; }
    }
}
