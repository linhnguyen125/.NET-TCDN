using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Infrastructure
{
    public class ConnectionString
    {
        public string GetConnectionString()
        {
            try
            {
                //get file appsettings.json từ project WebApi
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false);
                IConfiguration configuration = builder.Build();

                //Lấy thông tin chuỗi kết nối
                string connectionString = configuration.GetValue<string>("ConnectionStrings:manhnv");
                return connectionString; //trả về chuỗi kết nối
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
