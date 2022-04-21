using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Entities
{
    public class FilterObject
    {
        public int page_size { get; set; }

        public int page_number { get; set; }

        public string? category { get; set; }

        public string? txt_search { get; set; }

        public string[] columns { get; set; }
    }
}
