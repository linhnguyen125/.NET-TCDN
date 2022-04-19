//using Dapper;
//using MISA.Core.Entities;
//using MISA.Core.Interfaces;
//using MySqlConnector;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace MISA.Infrastructure.Repository
//{
//    /// <summary>
//    /// Thực hiện làm việc với dữ liệu nhân viên
//    /// </summary>
//    /// CreatedBy: NVLINH (09/03/2022)
//    public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
//    {
//        /// <summary>
//        /// Thực hiện lấy dữ liệu nhân viên có phân trang
//        /// </summary>
//        /// <param name="pageSize"></param>
//        /// <param name="pageNumber"></param>
//        /// <param name="txtSearch"></param>
//        /// <returns></returns>
//        /// CreatedBy: NVLINH (10/03/2022)
//        public Object GetPaging(int pageSize, int pageNumber, string? txtSearch)
//        {
//            using (var sqlConnection = new MySqlConnection(ConnectionString))
//            {
//                if (string.IsNullOrEmpty(txtSearch))
//                {
//                    txtSearch = "";
//                }
//                //tạo các parameter gán dữ liệu từ client truyền vào
//                DynamicParameters parameters = new DynamicParameters();
//                parameters.Add("@txtSearch", "%" + txtSearch + "%");
//                parameters.Add("@Offset", (pageNumber - 1) * pageSize);
//                parameters.Add("@Limit", pageSize);

//                //1.câu lệnh truy vấn số bản ghi phù hợp với employeeFilter
//                var sqlCommand = @$"SELECT COUNT(1) FROM Employee WHERE EmployeeCode LIKE @txtSearch OR FullName LIKE @txtSearch OR PhoneNumber LIKE @txtSearch;
//                                SELECT * FROM Employee WHERE EmployeeCode LIKE @txtSearch OR FullName LIKE @txtSearch OR PhoneNumber LIKE @txtSearch ORDER BY CreatedDate DESC LIMIT @Offset, @Limit";

//                //thực hiện query multiple
//                var multi = sqlConnection.QueryMultiple(sqlCommand, param: parameters);
//                //trả về tổng số bản ghi
//                var totalRecord = multi.Read<int>().Single();
                
//                //trả về danh sách nhân viên
//                var entities = multi.Read<Employee>().ToList();

//                //tính tổng số trang
//                double totalPage;
//                if (totalRecord < pageSize)
//                {
//                    totalPage = 1;
//                }
//                else
//                {
//                    totalPage = (double)totalRecord / pageSize; //tổng số lượng các trang
//                    if (totalPage != Math.Floor(totalPage)) //nếu chia có dư => số lượng page + 1
//                    {
//                        totalPage = Math.Floor(totalPage) + 1;
//                    }
//                }

//                //build Data gửi về cho client
//                var data = new
//                {
//                    Data = entities,
//                    TotalRecord = totalRecord,
//                    TotalPage = totalPage
//                };
//                //trả kết quả cho client
//                return data;
//            }
//        }

//        /// <summary>
//        /// Thực hiện lấy mã nhân viên mới
//        /// </summary>
//        /// <returns></returns>
//        /// CreatedBy: NVLINH (11/03/2022)
//        public string GetNewEmployeeCode()
//        {
//            string prefix = "MS";
//            var postFix = 0;
//            using (var sqlConnection = new MySqlConnection(ConnectionString))
//            {
//                // Câu lệnh truy vấn
//                var sqlCommand = "SELECT REGEXP_SUBSTR(EmployeeCode," + "'[0-9]+'" + ") as amount FROM Employee";
//                // Lấy dữ liệu
//                var res = sqlConnection.Query<int>(sql: sqlCommand);
//                // Lấy số lớn nhất trong hệ thống + 1
//                int max = res.Max();
//                postFix = max + 1;
//                // Trả về mã mới nhất tăng thêm 1
//                return prefix + postFix.ToString();
//            }
//        }

//        /// <summary>
//        /// Thực hiện thêm nhân viên vào database
//        /// </summary>
//        /// <param name="employee"></param>
//        /// <returns></returns>
//        /// CreatedBy: NVLINH (09/03/2022)
//        public override int Insert(Employee employee)
//        {
//            using (var sqlConnection = new MySqlConnection(ConnectionString))
//            {
//                //2. Tạo id và ngày tạo
//                employee.EmployeeId = Guid.NewGuid();
//                employee.CreatedDate = DateTime.UtcNow;
//                //3. Khởi tạo dynamic params
//                DynamicParameters parameters = new DynamicParameters();
//                //4. Thực hiện thêm mới dữ liệu vào database
//                //4.1 Gọi proc
//                var sqlCommandText = "Proc_InsertEmployee";
//                //4.2 Mở kết nối tới database
//                sqlConnection.Open();
//                //4.3 Đọc các tham số đầu vào của store
//                var sqlcommand = sqlConnection.CreateCommand();
//                sqlcommand.CommandText = sqlCommandText;
//                sqlcommand.CommandType = System.Data.CommandType.StoredProcedure;
//                MySqlCommandBuilder.DeriveParameters(sqlcommand);

//                foreach (MySqlParameter param in sqlcommand.Parameters)
//                {
//                    // Tên của tham số
//                    var paramName = param.ParameterName;
//                    // Tên của Property
//                    var propertyName = paramName.Replace("@ms_", "");
//                    // Lấy giá trị của property
//                    var entityProperty = employee.GetType().GetProperty(propertyName);
//                    if (entityProperty != null)
//                    {
//                        var propertyValue = entityProperty.GetValue(employee);
//                        // Thực hiện gán giá trị cho các params
//                        parameters.Add(paramName, propertyValue);
//                    }
//                    else
//                    {
//                        // Thực hiện gán giá trị cho các params
//                        parameters.Add(paramName, null);
//                    }
//                }

//                var res = sqlConnection.Execute(sql: sqlCommandText, param: parameters, commandType: System.Data.CommandType.StoredProcedure);
//                // Đóng kết nối database
//                sqlConnection.Clone();
//                return res;
//            }
//        }

//        /// <summary>
//        /// Thực hiện cập nhật nhân viên
//        /// </summary>
//        /// <param name="employeeId"></param>
//        /// <param name="employee"></param>
//        /// <returns></returns>
//        /// CreatedBy: NVLINH (09/03/2022)
//        public override int Update(Employee employee)
//        {
//            using (var sqlConnection = new MySqlConnection(ConnectionString))
//            {
//                //2. Tạo id và ngày sửa
//                employee.ModifiedDate = DateTime.UtcNow;
//                //3. Khởi tạo dynamic params
//                DynamicParameters parameters = new DynamicParameters();
//                //4. Thực hiện update dữ liệu vào database
//                //4.1 Gọi proc
//                var sqlCommandText = "Proc_UpdateEmployee";
//                //4.2 Mở kết nối tới database
//                sqlConnection.Open();
//                //4.3 Đọc các tham số đầu vào của store
//                var sqlcommand = sqlConnection.CreateCommand();
//                sqlcommand.CommandText = sqlCommandText;
//                sqlcommand.CommandType = System.Data.CommandType.StoredProcedure;
//                MySqlCommandBuilder.DeriveParameters(sqlcommand);

//                foreach (MySqlParameter param in sqlcommand.Parameters)
//                {
//                    // Tên của tham số
//                    var paramName = param.ParameterName;
//                    // Tên của Property
//                    var propertyName = paramName.Replace("@ms_", "");
//                    // Lấy giá trị của property
//                    var entityProperty = employee.GetType().GetProperty(propertyName);
//                    if (entityProperty != null)
//                    {
//                        var propertyValue = entityProperty.GetValue(employee);
//                        // Thực hiện gán giá trị cho các params
//                        parameters.Add(paramName, propertyValue);
//                    }
//                    else
//                    {
//                        // Thực hiện gán giá trị cho các params
//                        parameters.Add(paramName, null);
//                    }
//                }

//                var res = sqlConnection.Execute(sql: sqlCommandText, param: parameters, commandType: System.Data.CommandType.StoredProcedure);
//                // Đóng kết nối database
//                sqlConnection.Clone();
//                return res;
//            }
//        }
//    }
//}
