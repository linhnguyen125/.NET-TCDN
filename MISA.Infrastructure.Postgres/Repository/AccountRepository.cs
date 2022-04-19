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
    public class AccountRepository : BaseRepository<Account>, IAccountRepository
    {
        /// <summary>
        /// Thực hiện lấy dữ liệu nhân viên có phân trang
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="txtSearch"></param>
        /// <returns></returns>
        /// CreatedBy: NVLINH (10/03/2022)
        public Object GetPaging(int pageSize, int pageNumber, string? txtSearch)
        {
            using (var npgConnection = new NpgsqlConnection(ConnectionString))
            {
                if (string.IsNullOrEmpty(txtSearch))
                {
                    txtSearch = "";
                }
                //tạo các parameter gán dữ liệu từ client truyền vào
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@txtSearch", "%" + txtSearch + "%");
                parameters.Add("@Offset", (pageNumber - 1) * pageSize);
                parameters.Add("@Limit", pageSize);

                //1.câu lệnh truy vấn số bản ghi phù hợp với AccountFilter
                var sqlCommand = @$"SELECT COUNT(1) FROM account a WHERE a.*::text ILIKE @txtSearch;
                                SELECT * FROM account a WHERE a.*::text ILIKE @txtSearch ORDER BY created_date DESC LIMIT @Limit OFFSET @Offset";

                //thực hiện query multiple
                var multi = npgConnection.QueryMultiple(sqlCommand, param: parameters);
                //trả về tổng số bản ghi
                var totalRecord = multi.Read<int>().Single();

                //trả về danh sách nhân viên
                var entities = multi.Read<Account>().ToList();

                //tính tổng số trang
                double totalPage;
                if (totalRecord < pageSize)
                {
                    totalPage = 1;
                }
                else
                {
                    totalPage = (double)totalRecord / pageSize; //tổng số lượng các trang
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
        /// Kiểm tra mã trùng create
        /// </summary>
        /// <param name="entityCode"></param>
        /// <returns>
        /// true - đã bị trùng, false - không bị trùng
        /// </returns>
        /// CreatedBy NVLINH (11/04/2022)
        public virtual bool CheckDuplicateCode(string accountNumber)
        {
            using (var npgConnection = new NpgsqlConnection(ConnectionString))
            {
                var sqlCommand = $"SELECT account_number FROM {_tableName} WHERE account_number = @account_number";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add($"account_number", accountNumber);
                var res = npgConnection.QueryFirstOrDefault<string>(sql: sqlCommand, param: parameters);
                if (res == null)
                    return false;
                return true;
            }
        }

        /// <summary>
        /// Kiểm tra mã trùng update
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="entityCode"></param>
        /// <returns>
        /// true - đã bị trùng, false - không bị trùng
        /// </returns>
        /// CreatedBy NVLINH (11/04/2022)
        public virtual bool CheckDuplicateCode(Guid accountId, string accountNumber)
        {
            using (var npgConnection = new NpgsqlConnection(ConnectionString))
            {
                var sqlCommand = $"SELECT account_number FROM {_tableName} WHERE account_number = @account_number AND {_tableName}_id NOT IN (@{_tableName}_id)";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add($"account_number", accountNumber);
                parameters.Add($"@{_tableName}_id", accountId);
                var res = npgConnection.QueryFirstOrDefault<string>(sql: sqlCommand, param: parameters);
                if (res == null)
                    return false;
                return true;
            }
        }
    }
}
