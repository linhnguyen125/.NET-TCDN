using Dapper;
using MISA.Core.Entities;
using MISA.Core.Interfaces;
using MISA.Infrastructure.Postgres.Repository;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Infrastructure.Postgres.Repository
{
    /// <summary>
    /// Thực hiện làm việc với dữ liệu nhân viên
    /// </summary>
    /// CreatedBy: NVLINH (09/03/2022)
    public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
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

                //1.câu lệnh truy vấn số bản ghi phù hợp với employeeFilter
                var sqlCommand = @$"SELECT COUNT(1) FROM employee e WHERE e.*::text ILIKE @txtSearch;
                                SELECT * FROM employee e WHERE e.*::text ILIKE @txtSearch ORDER BY created_date DESC LIMIT @Limit OFFSET @Offset";

                //thực hiện query multiple
                var multi = npgConnection.QueryMultiple(sqlCommand, param: parameters);
                //trả về tổng số bản ghi
                var totalRecord = multi.Read<int>().Single();

                //trả về danh sách nhân viên
                var entities = multi.Read<Employee>().ToList();

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
        /// Thực hiện lấy mã nhân viên mới
        /// </summary>
        /// <returns></returns>
        /// CreatedBy: NVLINH (11/03/2022)
        public string GetNewEmployeeCode()
        {
            string prefix = "MS";
            var postFix = 0;
            using (var npgConnection = new NpgsqlConnection(ConnectionString))
            {
                // Câu lệnh truy vấn
                var sqlCommand = "SELECT substring(employee_code, '[0-9]+') as amount FROM Employee";
                // Lấy dữ liệu
                var res = npgConnection.Query<int>(sql: sqlCommand);
                // Lấy số lớn nhất trong hệ thống + 1
                int max = res.Max();
                postFix = max + 1;
                // Trả về mã mới nhất tăng thêm 1
                return prefix + postFix.ToString();
            }
        }
    }
}
