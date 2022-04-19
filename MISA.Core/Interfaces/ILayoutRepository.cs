using MISA.Core.Entities;
using MISA.Core.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Interfaces
{
    public interface ILayoutRepository : IBaseRepository<Layout>
    {
        public Object GetLayout(bool is_default, string layout_code);

        public int UpdateLayout(Layout layout);
    }
}
