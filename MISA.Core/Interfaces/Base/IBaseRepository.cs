using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Interfaces.Base
{
    public interface IBaseRepository<MISAEntity> where MISAEntity : class
    {
        /// <summary>
        /// Lấy danh sách 
        /// </summary>
        /// <returns></returns>
        /// CreatedBy: NVLINH (09/03/2022)
        public IEnumerable<MISAEntity> Get();

        /// <summary>
        /// Lấy theo Id
        /// </summary>
        /// <returns></returns>
        /// CreatedBy: NVLINH (09/03/2022)
        public MISAEntity GetById(Guid entityId);

        /// <summary>
        /// Thêm mới
        /// </summary>
        /// <returns></returns>
        /// CreatedBy: NVLINH (09/03/2022)
        public int Insert(MISAEntity entity);

        /// <summary>
        /// Cập nhật
        /// </summary>
        /// <returns></returns>
        /// CreatedBy: NVLINH (09/03/2022)
        public int Update(MISAEntity entity);

        /// <summary>
        /// Xóa theo id
        /// </summary>
        /// <returns></returns>
        /// CreatedBy: NVLINH (09/03/2022)
        public int Delete(Guid entityId);

        /// <summary>
        /// Xóa nhiều
        /// </summary>
        /// <returns></returns>
        /// CreatedBy: NVLINH (09/03/2022)
        public int Delete(Guid[] entityIds);

        /// <summary>
        /// Thực hiện check mã insert
        /// </summary>
        /// <returns></returns>
        /// CreatedBy: NVLINH (09/03/2022)
        public bool CheckDuplicateCode(string entityCode);

        /// <summary>
        /// Thực hiện check mã khi update
        /// </summary>
        /// <returns></returns>
        /// CreatedBy: NVLINH (09/03/2022)
        public bool CheckDuplicateCode(Guid entityId, string entityCode);
    }
}
