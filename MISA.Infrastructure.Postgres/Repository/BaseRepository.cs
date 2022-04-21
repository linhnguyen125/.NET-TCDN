using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MISA.Core.Interfaces.Base;
using Npgsql;
using System.Reflection;
using MISA.Core.Entities;

namespace MISA.Infrastructure.Postgres.Repository
{
    public class BaseRepository<MISAEntity> : IBaseRepository<MISAEntity> where MISAEntity : class
    {
        // Chuỗi kết nối
        // protected string ConnectionString = "Host=3.0.89.182; Port=3306; Database = MISA.WEB01.NVLinh; User Id = dev; Password = 12345678";
        // Tên của bảng dữ liệu
        protected string ConnectionString = new ConnectionString().GetConnectionString();
        protected string _tableName = string.Empty;

        public BaseRepository()
        {
            // Lấy tên của class (bảng dữ liệu)
            _tableName = ToUnderscoreCase(typeof(MISAEntity).Name);
        }

        /// <summary>
        /// Lấy toàn bộ dữ liệu
        /// </summary>
        /// <typeparam name="MISAEntity"></typeparam>
        /// <returns></returns>
        /// CreatedBy: NVLINH (11/03/2022)
        public IEnumerable<MISAEntity> Get()
        {
            using (var npgConnection = new NpgsqlConnection(ConnectionString))
            {
                // Câu lệnh truy vấn
                var sqlCommand = $"SELECT * FROM {_tableName}";
                // Lấy dữ liệu
                var entities = npgConnection.Query<MISAEntity>(sql: sqlCommand);
                // Trả về dữ liệu
                return entities;
            }
        }

        /// <summary>
        /// Thêm mới dữ liệu
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// CreatedBy: NVLINH (11/03/2022)
        public virtual int Insert(MISAEntity entity)
        {
            using (var npgConnection = new NpgsqlConnection(ConnectionString))
            {
                // Tạo id và ngày tạo
                entity.GetType().GetProperty($"{_tableName}_id").SetValue(entity, Guid.NewGuid());
                entity.GetType().GetProperty("created_date").SetValue(entity, DateTime.UtcNow);

                // CÁCH 1: Dùng Function / Procedure
                //// Gọi proc
                //var sqlCommandText = $"proc_insert_{_tableName}";
                //// Khởi tạo dynamic params
                //DynamicParameters parameters = new DynamicParameters();
                //// Mở kết nối tới database
                //npgConnection.Open();
                //// Đọc các tham số đầu vào của store
                //var sqlcommand = npgConnection.CreateCommand();
                //sqlcommand.CommandText = sqlCommandText;
                //sqlcommand.CommandType = System.Data.CommandType.StoredProcedure;
                //NpgsqlCommandBuilder.DeriveParameters(sqlcommand);

                //foreach (NpgsqlParameter param in sqlcommand.Parameters)
                //{
                //    // Tên của tham số
                //    var paramName = param.ParameterName;
                //    // Tên của Property
                //    var propertyName = paramName.Replace("ms_", "");
                //    // Lấy giá trị của property
                //    var entityProperty = entity.GetType().GetProperty(propertyName);
                //    if (entityProperty != null)
                //    {
                //        var propertyValue = entityProperty.GetValue(entity);
                //        // Thực hiện gán giá trị cho các params
                //        parameters.Add(paramName, propertyValue);
                //    }
                //    else
                //    {
                //        // Thực hiện gán giá trị cho các params
                //        parameters.Add(paramName, null);
                //    }
                //}

                //var res = npgConnection.Execute(sql: sqlCommandText, param: parameters, commandType: System.Data.CommandType.StoredProcedure);
                //// Đóng kết nối database
                //npgConnection.Close();
                //// Trả về kết quả
                //return res;

                // CÁCH 2: Dùng query
                var columns = GetTableColumns();
                var sqlCommand = new StringBuilder();
                sqlCommand.Append($"INSERT INTO {_tableName}(");
                for (var i = 0; i < columns.Count(); i++)
                {
                    if (i > 0) { sqlCommand.Append(", "); }
                    sqlCommand.Append($"{columns[i]}");
                }
                sqlCommand.Append(") VALUES(");
                for (var i = 0; i < columns.Count(); i++)
                {
                    if (i > 0) { sqlCommand.Append(", "); }
                    sqlCommand.Append($"@{columns[i]}");
                }
                sqlCommand.Append(")");

                // Thêm dữ liệu vào DB:
                //Câu lệnh truy vấn dữ liệu:
                var res = npgConnection.Execute(sql: sqlCommand.ToString(), param: entity);
                return res;
            }
        }
        public virtual int Insert(MISAEntity entity, NpgsqlTransaction transaction)
        {
            using (var npgConnection = new NpgsqlConnection(ConnectionString))
            {
                // Tạo id và ngày tạo
                entity.GetType().GetProperty($"{_tableName}_id").SetValue(entity, Guid.NewGuid());
                entity.GetType().GetProperty("created_date").SetValue(entity, DateTime.UtcNow);

                // CÁCH 1: Dùng Function / Procedure
                //// Gọi proc
                //var sqlCommandText = $"proc_insert_{_tableName}";
                //// Khởi tạo dynamic params
                //DynamicParameters parameters = new DynamicParameters();
                //// Mở kết nối tới database
                //npgConnection.Open();
                //// Đọc các tham số đầu vào của store
                //var sqlcommand = npgConnection.CreateCommand();
                //sqlcommand.CommandText = sqlCommandText;
                //sqlcommand.CommandType = System.Data.CommandType.StoredProcedure;
                //NpgsqlCommandBuilder.DeriveParameters(sqlcommand);

                //foreach (NpgsqlParameter param in sqlcommand.Parameters)
                //{
                //    // Tên của tham số
                //    var paramName = param.ParameterName;
                //    // Tên của Property
                //    var propertyName = paramName.Replace("ms_", "");
                //    // Lấy giá trị của property
                //    var entityProperty = entity.GetType().GetProperty(propertyName);
                //    if (entityProperty != null)
                //    {
                //        var propertyValue = entityProperty.GetValue(entity);
                //        // Thực hiện gán giá trị cho các params
                //        parameters.Add(paramName, propertyValue);
                //    }
                //    else
                //    {
                //        // Thực hiện gán giá trị cho các params
                //        parameters.Add(paramName, null);
                //    }
                //}

                //var res = npgConnection.Execute(sql: sqlCommandText, param: parameters, commandType: System.Data.CommandType.StoredProcedure);
                //// Đóng kết nối database
                //npgConnection.Close();
                //// Trả về kết quả
                //return res;

                // CÁCH 2: Dùng query
                var columns = GetTableColumns();
                var sqlCommand = new StringBuilder();
                sqlCommand.Append($"INSERT INTO {_tableName}(");
                for (var i = 0; i < columns.Count(); i++)
                {
                    if (i > 0) { sqlCommand.Append(", "); }
                    sqlCommand.Append($"{columns[i]}");
                }
                sqlCommand.Append(") VALUES(");
                for (var i = 0; i < columns.Count(); i++)
                {
                    if (i > 0) { sqlCommand.Append(", "); }
                    sqlCommand.Append($"@{columns[i]}");
                }
                sqlCommand.Append(")");

                // Thêm dữ liệu vào DB:
                //Câu lệnh truy vấn dữ liệu:
                var res = npgConnection.Execute(sql: sqlCommand.ToString(), param: entity, transaction: transaction);
                return res;
            }
        }

        /// <summary>
        /// Lấy thông tin theo Id
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        /// CreatedBy: NVLINH (11/03/2022)
        public MISAEntity GetById(Guid entityId)
        {
            using (var npgConnection = new NpgsqlConnection(ConnectionString))
            {
                // Câu lệnh truy vấn
                var sqlCommand = $"SELECT * FROM {_tableName} WHERE {_tableName}_id = @{_tableName}_id";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add($"@{_tableName}_id", entityId);
                // Lấy dữ liệu
                var entity = npgConnection.QueryFirstOrDefault<MISAEntity>(sql: sqlCommand, param: parameters);
                return entity;
            }
        }

        /// <summary>
        /// Xóa dữ liệu
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        /// CreatedBy: NVLINH (11/03/2022)
        public int Delete(Guid entityId)
        {
            using (var npgConnection = new NpgsqlConnection(ConnectionString))
            {
                // Câu lệnh truy vấn
                var sqlCommand = $"DELETE FROM {_tableName} WHERE {_tableName}_id = @{_tableName}_id";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add($"@{_tableName}_id", entityId);
                // Lấy dữ liệu
                var entity = npgConnection.Execute(sql: sqlCommand, param: parameters);
                // Trả về dữ liệu
                return entity;
            }
        }

        /// <summary>
        /// Thực hiện cập nhật dữ liệu
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// CreatedBy: NVLINH (11/03/2022)
        public virtual int Update(MISAEntity entity)
        {
            using (var npgConnection = new NpgsqlConnection(ConnectionString))
            {
                // Tạo id và ngày tạo
                entity.GetType().GetProperty("modified_date").SetValue(entity, DateTime.UtcNow);

                // CÁCH 1: Dùng function / procedure
                //// Gọi proc
                //var sqlCommandText = $"proc_update_{_tableName}";
                //// Khởi tạo dynamic params
                //DynamicParameters parameters = new DynamicParameters();
                //// Mở kết nối tới database
                //npgConnection.Open();
                //// Đọc các tham số đầu vào của store
                //var sqlcommand = npgConnection.CreateCommand();
                //sqlcommand.CommandText = sqlCommandText;
                //sqlcommand.CommandType = System.Data.CommandType.StoredProcedure;
                //NpgsqlCommandBuilder.DeriveParameters(sqlcommand);

                //foreach (NpgsqlParameter param in sqlcommand.Parameters)
                //{
                //    // Tên của tham số
                //    var paramName = param.ParameterName;
                //    // Tên của Property
                //    var propertyName = paramName.Replace("ms_", "");
                //    // Lấy giá trị của property
                //    var entityProperty = entity.GetType().GetProperty(propertyName);
                //    if (entityProperty != null)
                //    {
                //        var propertyValue = entityProperty.GetValue(entity);
                //        // Thực hiện gán giá trị cho các params
                //        parameters.Add(paramName, propertyValue);
                //    }
                //    else
                //    {
                //        // Thực hiện gán giá trị cho các params
                //        parameters.Add(paramName, null);
                //    }
                //}

                //var res = npgConnection.Execute(sql: sqlCommandText, param: parameters, commandType: System.Data.CommandType.StoredProcedure);
                //// Đóng kết nối database
                //npgConnection.Close();
                //// Trả về kết quả
                //return res;

                // CÁCH 2: Dùng query
                var key = GetTableKey();
                var columns = GetTableColumns();
                var sqlCommand = new StringBuilder();
                sqlCommand.Append($"UPDATE {_tableName} SET");
                var hasColumn = false;
                for (var i = 0; i < columns.Count(); i++)
                {
                    var column = columns[i];
                    if (column == key)
                    {
                        continue;
                    }

                    if (hasColumn) { sqlCommand.Append(","); }
                    sqlCommand.Append($" {column}=@{column}");
                    hasColumn = true;
                }
                sqlCommand.Append($" WHERE {key}= @{key}");
                // Thêm dữ liệu từ DB:
                //Câu lệnh truy vấn dữ liệu:
                var res = npgConnection.Execute(sql: sqlCommand.ToString(), param: entity);
                return res;
            }
        }

        /// <summary>
        /// Thực hiện xóa dữ liệu
        /// </summary>
        /// <param name="entityIds"></param>
        /// <returns></returns>
        /// CreatedBy: NVLINH (11/03/2022)
        public int Delete(Guid[] entityIds)
        {
            using (var npgConnection = new NpgsqlConnection(ConnectionString))
            {
                // Câu lệnh truy vấn
                var sqlCommand = $"DELETE FROM {_tableName} WHERE {_tableName}_id IN (";
                DynamicParameters parameters = new DynamicParameters();
                // xử lý nỗi chuỗi sqlcommand
                for (int i = 0; i < entityIds.Length; i++)
                {
                    if (i > 0) sqlCommand += ", ";
                    sqlCommand += $"@param{i}";
                    parameters.Add($"@param{i}", entityIds[i]);

                }
                sqlCommand += ")";
                // Lấy dữ liệu
                var entity = npgConnection.Execute(sql: sqlCommand, param: parameters);
                // Trả về dữ liệu
                return entity;
            }
        }

        /// <summary>
        /// Kiểm tra mã trùng create
        /// </summary>
        /// <param name="entityCode"></param>
        /// <returns>
        /// true - đã bị trùng, false - không bị trùng
        /// </returns>
        /// CreatedBy NVLINH (09/03/2022)
        public virtual bool CheckDuplicateCode(string entityCode)
        {
            using (var npgConnection = new NpgsqlConnection(ConnectionString))
            {
                var sqlCommand = $"SELECT {_tableName}_code FROM {_tableName} WHERE {_tableName}_code = @{_tableName}_code";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add($"@{_tableName}_code", entityCode);
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
        /// CreatedBy NVLINH (09/03/2022)
        public virtual bool CheckDuplicateCode(Guid entityId, string entityCode)
        {
            using (var npgConnection = new NpgsqlConnection(ConnectionString))
            {
                var sqlCommand = $"SELECT {_tableName}_code FROM {_tableName} WHERE {_tableName}_code = @{_tableName}_code AND {_tableName}_id NOT IN (@{_tableName}_id)";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add($"@{_tableName}_code", entityCode);
                parameters.Add($"@{_tableName}_id", entityId);
                var res = npgConnection.QueryFirstOrDefault<string>(sql: sqlCommand, param: parameters);
                if (res == null)
                    return false;
                return true;
            }
        }

        /// <summary>
        /// Hàm lấy ra các cột map với property của Class
        /// </summary>
        /// <returns></returns>
        /// CreatedBy NVLINH (07/04/2022)
        public List<string> GetTableColumns()
        {
            var columns = new List<string>();
            var prs = typeof(MISAEntity).GetProperties();
            foreach (var p in prs)
            {
                var att = p.GetCustomAttribute<MISAColumn>();
                if (att != null)
                {
                    columns.Add(p.Name);
                }
            }
            return columns;
        }

        /// <summary>
        /// Hàm lấy khóa chính của db map với property của class
        /// </summary>
        /// <returns></returns>
        /// CreatedBy NVLINH (07/04/2022)
        public string GetTableKey()
        {
            var prs = typeof(MISAEntity).GetProperties();
            foreach (var p in prs)
            {
                var att = p.GetCustomAttribute<MISAKey>();
                if (att != null)
                {
                    return p.Name;
                }
            }
            return null;
        }

        public string ToUnderscoreCase(string str)
        {
            return string.Concat(str.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString())).ToLower();
        }
    }
}
