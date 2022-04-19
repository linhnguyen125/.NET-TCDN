//using MySqlConnector;
//using Dapper;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using MISA.Core.Interfaces.Base;

//namespace MISA.Infrastructure.Postgres.Repository
//{
//    public class BaseRepository<MISAEntity> : IBaseRepository<MISAEntity> where MISAEntity : class
//    {
//        // Chuỗi kết nối
//        // protected string ConnectionString = "Host=3.0.89.182; Port=3306; Database = MISA.WEB01.NVLinh; User Id = dev; Password = 12345678";
//        // Tên của bảng dữ liệu
//        protected string ConnectionString = new ConnectionString().GetConnectionString();
//        private string _tableName = string.Empty;

//        public BaseRepository()
//        {
//            // Lấy tên của class (bảng dữ liệu)
//            _tableName = typeof(MISAEntity).Name;
//        }

//        /// <summary>
//        /// Lấy toàn bộ dữ liệu
//        /// </summary>
//        /// <typeparam name="MISAEntity"></typeparam>
//        /// <returns></returns>
//        /// CreatedBy: NVLINH (11/03/2022)
//        public IEnumerable<MISAEntity> Get()
//        {
//            using (var sqlConnection = new MySqlConnection(ConnectionString))
//            {
//                // Câu lệnh truy vấn
//                var sqlCommand = $"SELECT * FROM {_tableName} ORDER BY CreatedDate DESC";
//                // Lấy dữ liệu
//                var entities = sqlConnection.Query<MISAEntity>(sql: sqlCommand);
//                // Trả về dữ liệu
//                return entities;
//            }
//        }

//        /// <summary>
//        /// Thêm mới dữ liệu
//        /// </summary>
//        /// <param name="entity"></param>
//        /// <returns></returns>
//        /// CreatedBy: NVLINH (11/03/2022)
//        public virtual int Insert(MISAEntity entity)
//        {
//            using (var sqlConnection = new MySqlConnection(ConnectionString))
//            {
//                // Tạo id và ngày tạo
//                entity.GetType().GetProperty($"{_tableName}Id").SetValue(entity, Guid.NewGuid());
//                entity.GetType().GetProperty("CreatedDate").SetValue(entity, DateTime.UtcNow);
//                // Gọi proc
//                var sqlCommandText = $"Proc_Insert{_tableName}";
//                // Khởi tạo dynamic params
//                DynamicParameters parameters = new DynamicParameters();
//                // Mở kết nối tới database
//                sqlConnection.Open();
//                // Đọc các tham số đầu vào của store
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
//                    var entityProperty = entity.GetType().GetProperty(propertyName);
//                    if (entityProperty != null)
//                    {
//                        var propertyValue = entityProperty.GetValue(entity);
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
//                // Trả về kết quả
//                return res;
//            }
//        }

//        /// <summary>
//        /// Lấy thông tin theo Id
//        /// </summary>
//        /// <param name="entityId"></param>
//        /// <returns></returns>
//        /// CreatedBy: NVLINH (11/03/2022)
//        public MISAEntity GetById(Guid entityId)
//        {
//            using (var sqlConnection = new MySqlConnection(ConnectionString))
//            {
//                // Câu lệnh truy vấn
//                var sqlCommand = $"SELECT * FROM {_tableName} WHERE {_tableName}Id = @{_tableName}Id";
//                DynamicParameters parameters = new DynamicParameters();
//                parameters.Add($"@{_tableName}Id", entityId);
//                // Lấy dữ liệu
//                var entity = sqlConnection.QueryFirstOrDefault<MISAEntity>(sql: sqlCommand, param: parameters);
//                return entity;
//            }
//        }

//        /// <summary>
//        /// Xóa dữ liệu
//        /// </summary>
//        /// <param name="entityId"></param>
//        /// <returns></returns>
//        /// CreatedBy: NVLINH (11/03/2022)
//        public int Delete(Guid entityId)
//        {
//            using (var sqlConnection = new MySqlConnection(ConnectionString))
//            {
//                // Câu lệnh truy vấn
//                var sqlCommand = $"DELETE FROM {_tableName} WHERE {_tableName}Id = @{_tableName}Id";
//                DynamicParameters parameters = new DynamicParameters();
//                parameters.Add($"@{_tableName}Id", entityId);
//                // Lấy dữ liệu
//                var entity = sqlConnection.Execute(sql: sqlCommand, param: parameters);
//                // Trả về dữ liệu
//                return entity;
//            }
//        }

//        /// <summary>
//        /// Thực hiện cập nhật dữ liệu
//        /// </summary>
//        /// <param name="entityId"></param>
//        /// <param name="entity"></param>
//        /// <returns></returns>
//        /// CreatedBy: NVLINH (11/03/2022)
//        public virtual int Update(MISAEntity entity)
//        {
//            using (var sqlConnection = new MySqlConnection(ConnectionString))
//            {
//                // Tạo id và ngày tạo
//                entity.GetType().GetProperty("ModifiedDate").SetValue(entity, DateTime.UtcNow);
//                // Gọi proc
//                var sqlCommandText = $"Proc_Update{_tableName}";
//                // Khởi tạo dynamic params
//                DynamicParameters parameters = new DynamicParameters();
//                // Mở kết nối tới database
//                sqlConnection.Open();
//                // Đọc các tham số đầu vào của store
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
//                    var entityProperty = entity.GetType().GetProperty(propertyName);
//                    if (entityProperty != null)
//                    {
//                        var propertyValue = entityProperty.GetValue(entity);
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
//                // Trả về kết quả
//                return res;
//            }
//        }

//        /// <summary>
//        /// Thực hiện xóa dữ liệu
//        /// </summary>
//        /// <param name="entityIds"></param>
//        /// <returns></returns>
//        /// CreatedBy: NVLINH (11/03/2022)
//        public int Delete(Guid[] entityIds)
//        {
//            using (var sqlConnection = new MySqlConnection(ConnectionString))
//            {
//                // Câu lệnh truy vấn
//                var sqlCommand = $"DELETE FROM {_tableName} WHERE {_tableName}Id IN (";
//                DynamicParameters parameters = new DynamicParameters();
//                // xử lý nỗi chuỗi sqlcommand
//                for (int i = 0; i < entityIds.Length; i++)
//                {
//                    if (i != entityIds.Length - 1)
//                    {
//                        sqlCommand += $"@param{i}, ";
//                        parameters.Add($"@param{i}", entityIds[i]);
//                    }
//                    else
//                    {
//                        sqlCommand += $"@param{i}";
//                        parameters.Add($"@param{i}", entityIds[i]);
//                    }
//                }
//                sqlCommand += ")";
//                // Lấy dữ liệu
//                var entity = sqlConnection.Execute(sql: sqlCommand, param: parameters);
//                // Trả về dữ liệu
//                return entity;
//            }
//        }

//        /// <summary>
//        /// Kiểm tra mã trùng create
//        /// </summary>
//        /// <param name="entityCode"></param>
//        /// <returns>
//        /// true - đã bị trùng, false - không bị trùng
//        /// </returns>
//        /// CreatedBy NVLINH (09/03/2022)
//        public bool CheckDuplicateCode(string entityCode)
//        {
//            using (var sqlConnection = new MySqlConnection(ConnectionString))
//            {
//                var sqlCommand = $"SELECT {_tableName}Code FROM {_tableName} WHERE {_tableName}Code = @{_tableName}Code";
//                DynamicParameters parameters = new DynamicParameters();
//                parameters.Add($"@{_tableName}Code", entityCode);
//                var res = sqlConnection.QueryFirstOrDefault<string>(sql: sqlCommand, param: parameters);
//                if (res == null)
//                    return false;
//                return true;
//            }
//        }

//        /// <summary>
//        /// Kiểm tra mã trùng update
//        /// </summary>
//        /// <param name="entityId"></param>
//        /// <param name="entityCode"></param>
//        /// <returns>
//        /// true - đã bị trùng, false - không bị trùng
//        /// </returns>
//        /// CreatedBy NVLINH (09/03/2022)
//        public bool CheckDuplicateCode(Guid entityId, string entityCode)
//        {
//            using (var sqlConnection = new MySqlConnection(ConnectionString))
//            {
//                var sqlCommand = $"SELECT {_tableName}Code FROM {_tableName} WHERE {_tableName}Code = @{_tableName}Code AND {_tableName}Id NOT IN (@{_tableName}Id)";
//                DynamicParameters parameters = new DynamicParameters();
//                parameters.Add($"@{_tableName}Code", entityCode);
//                parameters.Add($"@{_tableName}Id", entityId);
//                var res = sqlConnection.QueryFirstOrDefault<string>(sql: sqlCommand, param: parameters);
//                if (res == null)
//                    return false;
//                return true;
//            }
//        }
//    }
//}
