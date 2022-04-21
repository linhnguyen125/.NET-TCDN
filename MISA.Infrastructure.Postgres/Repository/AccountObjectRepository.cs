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
    public class AccountObjectRepository : BaseRepository<AccountObject>, IAccountObjectRepository
    {
        /// <summary>
        /// Thực hiện lấy dữ liệu nhân viên có phân trang
        /// </summary>
        /// <param name="filterObject"></param>
        /// <returns></returns>
        /// CreatedBy: NVLINH (10/03/2022)
        public Object GetPaging(FilterObject filterObject)
        {
            using (var npgConnection = new NpgsqlConnection(ConnectionString))
            {
                if (string.IsNullOrEmpty(filterObject.txt_search))
                {
                    filterObject.txt_search = "";
                }
                StringBuilder sb = new StringBuilder();
                StringBuilder column = new StringBuilder();
                if (filterObject.columns.Length == 0)
                {
                    sb.Append("1 = 1");
                }
                else
                {
                    for (int i = 0; i < filterObject.columns.Length; i++)
                    {
                        if (i > 0)
                        {
                            sb.Append("OR ");
                            column.Append(", ");
                        }
                        sb.Append($"{filterObject.columns[i]} ILIKE @txtSearch ");
                        column.Append($"{filterObject.columns[i]}");
                    }
                }
                //tạo các parameter gán dữ liệu từ client truyền vào
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@txtSearch", "%" + filterObject.txt_search + "%");
                parameters.Add("@Offset", (filterObject.page_number - 1) * filterObject.page_size);
                parameters.Add("@Limit", filterObject.page_size);

                //1.câu lệnh truy vấn số bản ghi phù hợp với AccountFilter
                var sqlCommand = @$"SELECT COUNT(1) FROM account_object WHERE is_{filterObject.category} = true and ({sb.ToString()});
                                SELECT * FROM account_object WHERE is_{filterObject.category} = true and ({sb.ToString()}) LIMIT @Limit OFFSET @Offset";

                //thực hiện query multiple
                var multi = npgConnection.QueryMultiple(sqlCommand, param: parameters);
                //trả về tổng số bản ghi
                var totalRecord = multi.Read<int>().Single();

                //trả về danh sách nhân viên
                var entities = multi.Read<AccountObject>().ToList();

                //tính tổng số trang
                double totalPage;
                if (totalRecord < filterObject.page_size)
                {
                    totalPage = 1;
                }
                else
                {
                    totalPage = (double)totalRecord / filterObject.page_size; //tổng số lượng các trang
                    if (totalPage != Math.Floor(totalPage)) //nếu chia có dư => số lượng page + 1
                    {
                        totalPage = Math.Floor(totalPage) + 1;
                    }
                }

                //build Data gửi về cho client
                var data = new
                {
                    Data = entities,
                    TotalRecord = totalRecord,
                    TotalPage = totalPage
                };
                //trả kết quả cho client
                return data;
            }
        }

        /// <summary>
        /// Get theo account object code
        /// </summary>
        /// <param name="account_object_code"></param>
        /// <returns></returns>
        /// <returns></returns>
        /// CreatedBy: NVLINH (18/04/2022)
        public Object GetByCode(string account_object_code)
        {
            using (var npgConnection = new NpgsqlConnection(ConnectionString))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@accountObjectCode", account_object_code);

                var sqlCommand = $"SELECT * FROM {_tableName} WHERE {_tableName}_code = @accountObjectCode";
                var res = npgConnection.QueryFirstOrDefault<AccountObject>(sql: sqlCommand, param: parameters);

                return res;
            }
        }
    }
}
