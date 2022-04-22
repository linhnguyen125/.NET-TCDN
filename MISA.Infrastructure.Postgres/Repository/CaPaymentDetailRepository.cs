using Dapper;
using MISA.Core.Entities;
using MISA.Core.Interfaces;

using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Infrastructure.Postgres.Repository
{
    public class CaPaymentDetailRepository : BaseRepository<CaPaymentDetail>, ICaPaymentDetailRepository
    {
        /// <summary>
        /// Lấy chi tiết phiếu chi theo refid
        /// </summary>
        /// <param name="refid"></param>
        /// <returns></returns>
        /// <returns></returns>
        /// CreatedBy: NVLINH (18/04/2022)
        public Object GetByRefid(Guid refid)
        {
            using (var npgConnection = new NpgsqlConnection(ConnectionString))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@refid", refid);

                var sqlCommand = $"SELECT * FROM {_tableName} WHERE refid = @refid";
                var res = npgConnection.Query<CaPaymentDetail>(sql: sqlCommand, param: parameters);

                return res;
            }
        }
    }
}
