using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Entities
{
    public class Layout : BaseEntity
    {
        public Guid layout_id { get; set; }
        public string? layout_name { get; set; }
        public string? layout_code { get; set; }
        public string? template_content { get; set; }
        public bool? is_default { get; set; }
    }
}
