using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Entities
{
    public class CaPaymentDetail : BaseEntity
    {
        [MISAKey]
        [MISAColumn]
        [MISARequired]
        public Guid ca_payment_detail_id { get; set; }

        [MISAColumn]
        public string? account_object_code { get; set; }

        [MISAColumn]
        public Guid? account_object_id { get; set; }

        [MISAColumn]
        public string? account_object_name { get; set; }

        [MISAColumn]
        public decimal? amount_oc { get; set; }

        [MISAColumn]
        public string? credit_account { get; set; }

        [MISAColumn]
        public string debit_account { get; set; }

        [MISAColumn]
        public string? description { get; set; }

        [MISAColumn]
        public Guid? refid { get; set; }
    }
}
