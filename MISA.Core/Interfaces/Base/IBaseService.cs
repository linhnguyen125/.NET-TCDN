using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Interfaces.Base
{
    public interface IBaseService<MISAEntity> where MISAEntity : class
    {
        public int InsertService(MISAEntity entity);

        public int UpdateService(MISAEntity entity);
    }
}
