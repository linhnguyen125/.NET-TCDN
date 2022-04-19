using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Entities
{
    public class TableExport
    {
        /// <summary>
        /// Tên cột dữ liệu
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Key cột dữ liệu map với DB
        /// </summary>
        public string Key { get; set; }
    }
}
