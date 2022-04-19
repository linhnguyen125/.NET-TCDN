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
        public override int Insert(CaPaymentDetail payment)
        {
            // Tạo id và ngày tạo
            payment.GetType().GetProperty($"{_tableName}_id").SetValue(payment, Guid.NewGuid());
            payment.GetType().GetProperty("created_date").SetValue(payment, DateTime.UtcNow);

            using (var npgConnection = new NpgsqlConnection(ConnectionString))
            {
                npgConnection.Open();

                using (var transaction = npgConnection.BeginTransaction())
                {
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

                        // if it was successful, commit the transaction
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
    }
}
