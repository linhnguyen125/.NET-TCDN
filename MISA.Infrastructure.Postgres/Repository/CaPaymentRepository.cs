using Dapper;
using MISA.Core.Entities;
using MISA.Core.Exceptions;
using MISA.Core.Interfaces;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace MISA.Infrastructure.Postgres.Repository
{
    public class CaPaymentRepository : BaseRepository<CaPayment>, ICaPaymentRepository
    {
        //Khai báo biến chứa lỗi
        public Dictionary<string, string> errorData = new Dictionary<string, string>();
        /// <summary>
        /// Hàm thêm mới phiếu chi
        /// </summary>
        /// <param name="payment"></param>
        /// <returns></returns>
        /// CreatedBy: NVLINH(19/04/2022)
        public int InsertFull(CaPayment payment)
        {
            // Tạo id và ngày tạo
            var id = Guid.NewGuid();
            payment.ca_payment_id = id;
            payment.created_date = DateTime.UtcNow;
            // check trùng mã
            if (CheckDuplicateCode(payment.ca_payment_code))
            {
                errorData.Add($"{_tableName}_code", String.Format(Core.Resources.ResourceVN.ValidateError_DuplicateEntityCode, "Phiếu chi", payment.ca_payment_code));
                throw new ValidateException(Core.Resources.ResourceVN.ValidateError_Invalid, errorData);
            }

            using (var trxScope = new TransactionScope())
            {
                try
                {
                    var res = Insert(payment);

                    if (res > 0)
                    {
                        //nếu có list detail.
                        if (payment.ca_payment_detail != null)
                        {
                            CaPaymentDetailRepository _paymentDetailRepo = new CaPaymentDetailRepository();
                            var listDetail = payment.ca_payment_detail;

                            //for trong list để thêm mới, gán id của master vào ref_id của detail.
                            for (int i = 0; i < listDetail.Count; i++)
                            {
                                //khởi tạo mã mới cho detail.
                                listDetail[i].ca_payment_detail_id = Guid.NewGuid();
                                //gán mã của master cho detail
                                listDetail[i].refid = id;
                                //gán ngày tạo
                                listDetail[i].created_date = DateTime.UtcNow;
                                //gọi hàm thêm detail
                                _paymentDetailRepo.Insert(listDetail[i]);
                            }
                        }
                    }
                    trxScope.Complete();
                    return res;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Hàm cập nhật phiếu chi
        /// </summary>
        /// <param name="payment"></param>
        /// <returns></returns>
        /// CreatedBy: NVLINH(19/04/2022)
        public int UpdateFull(CaPayment payment)
        {
            if (CheckDuplicateCode(payment.ca_payment_id, payment.ca_payment_code))
            {
                errorData.Add($"{_tableName}_code", String.Format(Core.Resources.ResourceVN.ValidateError_DuplicateEntityCode, "Phiếu chi", payment.ca_payment_code));
            }
            using (var trxScope = new TransactionScope())
            {
                try
                {
                    payment.modified_date = DateTime.UtcNow;
                    // cập nhật bảng cho master
                    var res = Update(payment);

                    // Cập nhật bảng detail
                    if (res > 0)
                    {
                        if (payment.ca_payment_detail != null)
                        {
                            CaPaymentDetailRepository _paymentDetailRepo = new CaPaymentDetailRepository();
                            var listDetail = payment.ca_payment_detail;
                            //lấy hết danh sách detail đang tồn tại của master ra.
                            var list_detail_exist = _paymentDetailRepo.GetByRefid((Guid)payment.ca_payment_id);
                            //duyệt trong list danh sách detail của client gửi lên

                            //nếu flag = xóa => xóa trong db.
                            //nếu flag = thêm => thêm vào db.
                            //nếu flag = sửa => sửa trong db.

                            //for trong list đã tồn tại
                            for (int i = 0; i < list_detail_exist.Count(); i++)
                            {
                                var still_exist = false;
                                //nếu list client gửi lên không chứa đang tồn tại => xóa, nếu chứa => cập nhật
                                for (int j = 0; j < listDetail.Count; j++)
                                {
                                    if (list_detail_exist.ElementAt(i).ca_payment_detail_id == listDetail[j].ca_payment_detail_id)
                                    {
                                        still_exist = true;//vẫn đang tồn tại => gọi sửa
                                        listDetail[j].modified_date = DateTime.UtcNow;
                                        _paymentDetailRepo.Update(listDetail[j]);
                                    }
                                }
                                if (still_exist == false)
                                {
                                    //ko tồn tại thì gọi xóa
                                    _paymentDetailRepo.Delete((Guid)list_detail_exist.ElementAt(i).ca_payment_detail_id);
                                }
                            }

                            //for trong list gửi lên, nếu ko có thì thêm mới
                            for (int i = 0; i < listDetail.Count; i++)
                            {
                                var exist = false;
                                for (int j = 0; j < list_detail_exist.Count(); j++)
                                {
                                    if (list_detail_exist.ElementAt(j).ca_payment_detail_id == listDetail[i].ca_payment_detail_id)
                                    {
                                        exist = true;
                                        break;
                                    }
                                }
                                if (exist == false)//nếu chưa tồn tại thì thêm mới
                                {
                                    listDetail[i].ca_payment_detail_id = Guid.NewGuid();
                                    listDetail[i].refid = (Guid)payment.ca_payment_id;
                                    listDetail[i].created_date = DateTime.UtcNow;
                                    _paymentDetailRepo.Insert(listDetail[i]);
                                }
                            }
                        }
                    }
                    trxScope.Complete();
                    return res;
                }
                catch (Exception ex)
                {
                    throw ex;
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

        /// <summary>
        /// Thực hiện lấy mã nhân viên mới
        /// </summary>
        /// <returns></returns>
        /// CreatedBy: NVLINH (11/03/2022)
        public string GetNewPaymentCode()
        {
            string prefix = "PC";
            var postFix = 0;
            using (var npgConnection = new NpgsqlConnection(ConnectionString))
            {
                // Câu lệnh truy vấn
                var sqlCommand = $"SELECT substring(ca_payment_code, '[0-9]+') as amount FROM {_tableName}";
                // Lấy dữ liệu
                var res = npgConnection.Query<int>(sql: sqlCommand);
                // Lấy số lớn nhất trong hệ thống + 1
                int max = 0;
                if (res.Count() > 0)
                {
                    max = res.Max();
                }
                postFix = max + 1;
                // Trả về mã mới nhất tăng thêm 1
                return prefix + postFix;
            }
        }

        /// <summary>
        /// Kiểm tra mã trùng create
        /// </summary>
        /// <param name="payment_code"></param>
        /// <returns>
        /// true - đã bị trùng, false - không bị trùng
        /// </returns>
        /// CreatedBy NVLINH (11/04/2022)
        private bool CheckDuplicateCode(string payment_code)
        {
            using (var npgConnection = new NpgsqlConnection(ConnectionString))
            {
                var sqlCommand = $"SELECT {_tableName}_code FROM {_tableName} WHERE {_tableName}_code = @{_tableName}_code";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add($"{_tableName}_code", payment_code);
                var res = npgConnection.QueryFirstOrDefault<string>(sql: sqlCommand, param: parameters);
                if (res == null)
                    return false;
                return true;
            }
        }

        /// <summary>
        /// Kiểm tra mã trùng update
        /// </summary>
        /// <param name="payment_id"></param>
        /// <param name="payment_code"></param>
        /// <returns>
        /// true - đã bị trùng, false - không bị trùng
        /// </returns>
        /// CreatedBy NVLINH (11/04/2022)
        private bool CheckDuplicateCode(Guid payment_id, string payment_code)
        {
            using (var npgConnection = new NpgsqlConnection(ConnectionString))
            {
                var sqlCommand = $"SELECT {_tableName}_code FROM {_tableName} WHERE {_tableName}_code = @{_tableName}_code AND {_tableName}_id NOT IN (@{_tableName}_id)";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add($"{_tableName}_code", payment_code);
                parameters.Add($"@{_tableName}_id", payment_id);
                var res = npgConnection.QueryFirstOrDefault<string>(sql: sqlCommand, param: parameters);
                if (res == null)
                    return false;
                return true;
            }
        }
    }
}
