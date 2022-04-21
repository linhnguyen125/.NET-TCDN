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
    public class CaPaymentRepository : BaseRepository<CaPayment>, ICaPaymentRepository
    {
        /// <summary>
        /// Hàm thêm mới phiếu chi
        /// </summary>
        /// <param name="payment"></param>
        /// <returns></returns>
        /// CreatedBy: NVLINH(19/04/2022)
        public override int Insert(CaPayment payment)
        {
            // Tạo id và ngày tạo
            var id = Guid.NewGuid();
            payment.GetType().GetProperty($"{_tableName}_id").SetValue(payment, id);
            payment.GetType().GetProperty("created_date").SetValue(payment, DateTime.UtcNow);

            using (var npgConnection = new NpgsqlConnection(ConnectionString))
            {
                npgConnection.Open();

                using (var transaction = npgConnection.BeginTransaction())
                {
                    CaPaymentDetailRepository _paymentDetailRepo = new CaPaymentDetailRepository();
                    try
                    {
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

                        var res = npgConnection.Execute(sql: sqlCommand.ToString(), param: payment, transaction: transaction);

                        var columnsDetail = _paymentDetailRepo.GetTableColumns();

                        if (payment.ca_payment_detail.Count > 0)
                        {
                            foreach (var item in payment.ca_payment_detail)
                            {
                                item.ca_payment_detail_id = Guid.NewGuid();
                                item.refid = id;

                                var sqlCommandDetail = new StringBuilder();
                                sqlCommandDetail.Append($"INSERT INTO ca_payment_detail(");
                                for (var i = 0; i < columnsDetail.Count(); i++)
                                {
                                    if (i > 0) { sqlCommandDetail.Append(", "); }
                                    sqlCommandDetail.Append($"{columnsDetail[i]}");
                                }
                                sqlCommandDetail.Append(") VALUES(");
                                for (var i = 0; i < columnsDetail.Count(); i++)
                                {
                                    if (i > 0) { sqlCommandDetail.Append(", "); }
                                    sqlCommandDetail.Append($"@{columnsDetail[i]}");
                                }
                                sqlCommandDetail.Append(")");
                                npgConnection.Execute(sql: sqlCommandDetail.ToString(), param: item, transaction: transaction);
                            }
                        }

                        //var res = 1;

                        transaction.Commit();
                        return res;
                    }
                    catch (Exception ex)
                    {
                        // roll the transaction back
                        transaction.Rollback();

                        return 0;
                    }
                }

            }
        }

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
                var index = 0;
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
                        if (filterObject.columns[i] == "posted_date" || filterObject.columns[i] == "refdate")
                        {
                            continue;
                        }
                        index++;

                        if (index > 1)
                        {
                            sb.Append("OR ");
                            column.Append(", ");
                        }
                        sb.Append($"{filterObject.columns[i]}::text ILIKE @txtSearch ");
                        column.Append($"{filterObject.columns[i]}");
                    }
                }
                //tạo các parameter gán dữ liệu từ client truyền vào
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@txtSearch", "%" + filterObject.txt_search + "%");
                parameters.Add("@Offset", (filterObject.page_number - 1) * filterObject.page_size);
                parameters.Add("@Limit", filterObject.page_size);

                //1.câu lệnh truy vấn số bản ghi phù hợp với AccountFilter
                var sqlCommand = @$"SELECT COUNT(1) FROM {_tableName} WHERE {sb.ToString()};
                                SELECT * FROM {_tableName} WHERE {sb.ToString()} LIMIT @Limit OFFSET @Offset";

                //thực hiện query multiple
                var multi = npgConnection.QueryMultiple(sqlCommand, param: parameters);
                //trả về tổng số bản ghi
                var totalRecord = multi.Read<int>().Single();

                //trả về danh sách nhân viên
                var entities = multi.Read<CaPayment>().ToList();

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
    }
}
