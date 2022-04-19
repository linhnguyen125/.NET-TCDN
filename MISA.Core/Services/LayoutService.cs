using MISA.Core.Entities;
using MISA.Core.Interfaces;
using MISA.Core.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Services
{
    public class LayoutService : BaseService<Layout>, ILayoutService
    {
        ILayoutRepository _layoutRepository;
        public LayoutService(ILayoutRepository layoutRepository) : base(layoutRepository)
        {
            _layoutRepository = layoutRepository;
        }
    }
}
