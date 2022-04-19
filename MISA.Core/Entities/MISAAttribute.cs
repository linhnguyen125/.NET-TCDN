using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Entities
{
    /// <summary>
    /// Attribute đánh dấu thông tin bắt buộc nhập
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class MISARequired : Attribute
    {

    }

    /// <summary>
    /// Attribute validate Email đúng định dạng
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class MISAEmailValid : Attribute
    {

    }

    /// <summary>
    /// Attribute quy định display name cho thuộc tính
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class MISADisplayName : Attribute
    {
        public string DisplayName;

        public MISADisplayName(string propName)
        {
            this.DisplayName = propName;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class MISAMaxLength : Attribute
    {
        public int Length;
        public MISAMaxLength(int length)
        {
            this.Length = length;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class MISAColumn : Attribute
    {

    }

    [AttributeUsage(AttributeTargets.Property)]
    public class MISAKey : Attribute
    {

    }
}
