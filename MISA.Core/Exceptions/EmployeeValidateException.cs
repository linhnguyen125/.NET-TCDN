using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Exceptions
{
    public class EmployeeValidateException: Exception
    {
        public string ErrorMsg { get; set; }
        public Dictionary<string, string> ErrorData { get; set; }
        public EmployeeValidateException(string errorMsg, Dictionary<string, string> errorData)
        {
            this.ErrorMsg = errorMsg;
            this.ErrorData = errorData;
        }
        public override string Message => this.ErrorMsg;
        public override IDictionary Data => this.ErrorData;
    }
}
