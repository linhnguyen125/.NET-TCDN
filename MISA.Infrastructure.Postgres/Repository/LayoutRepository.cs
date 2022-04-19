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
    public class LayoutRepository : BaseRepository<Layout>, ILayoutRepository
    {
        public object GetLayout(bool is_default, string layout_code)
        {
            using (var npgConnection = new NpgsqlConnection(ConnectionString))
            {
                //1.câu lệnh truy vấn số bản ghi phù hợp với employeeFilter
                var sqlCommand = $"SELECT * FROM layout WHERE is_default = {is_default} and layout_code = '{layout_code}'";

                //thực hiện query multiple
                var res = npgConnection.QueryFirstOrDefault<Layout>(sqlCommand);

                //trả kết quả cho client
                return res;
            }
        }

        public int UpdateLayout(Layout layout)
        {
            using (var npgConnection = new NpgsqlConnection(ConnectionString))
            {
                //1.câu lệnh truy vấn số bản ghi phù hợp với employeeFilter
                var sqlCommand = $"UPDATE layout SET template_content = '{layout.template_content}' WHERE is_default = false and layout_code = '{layout.layout_code}'";

                //thực hiện query multiple
                var res = npgConnection.Execute(sqlCommand);

                //trả kết quả cho client
                return res;
            }
        }
    }
}
