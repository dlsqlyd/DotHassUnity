using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotHass.Unity
{
    public static class ConvertExtensions
    {        /// <summary>
             /// Sql最小时间范围
             /// </summary>
        public static DateTime SqlMinDate = new DateTime(1753, 1, 1);

        /// <summary>
        /// 同string.IsNullOrEmpty
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        /// <summary>
        /// 判断对象是否为空或DBNull或new object()对象
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNullOrDbNull(this object value)
        {
            return value == null || string.IsNullOrEmpty(Convert.ToString(value)) || value.GetType() == typeof(object);
        }



        #region 转换值

        /// <summary>
        /// 将对象转换成字符串，Null为string.Empty
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToNotNullString(this object value)
        {
            return ToNotNullString(value, string.Empty);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static string ToNotNullString(this object value, string defValue)
        {
            defValue = defValue.IsNullOrDbNull() ? string.Empty : defValue;
            return value.IsNullOrDbNull() || value.ToString().IsEmpty() ? defValue : value.ToString();
        }

        /// <summary>
        /// 向上取整
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ToCeilingInt(this object value)
        {
            if (value is decimal)
            {
                return (int)Math.Ceiling((decimal)value);
            }
            else if (value is double)
            {
                return (int)Math.Ceiling((double)value);
            }
            else if (value is float)
            {
                return (int)Math.Ceiling((float)value);
            }
            else
            {
                return (int)Math.Ceiling(ToDouble(value));
            }
        }

        /// <summary>
        /// 向下取整
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ToFloorInt(this object value)
        {
            if (value is decimal)
            {
                return (int)Math.Floor((decimal)value);
            }
            else if (value is double)
            {
                return (int)Math.Floor((double)value);
            }
            else if (value is float)
            {
                return (int)Math.Floor((float)value);
            }
            else
            {
                return (int)Math.Floor(ToDouble(value));
            }
        }

        /// <summary>
        /// 将对象转换成长整型值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static long ToLong(this object value)
        {
            try
            {
                return Convert.ToInt64(value.IsNullOrDbNull() ? 0 : value);
            }
            catch
            {
                throw new ArgumentException(string.Format("\"{0}\" converted to type long fail.", value));
            }
        }
        /// <summary>
        /// 将对象转换成整型值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ToInt(this object value)
        {
            try
            {
                if (value.IsNullOrDbNull() || false.Equals(value))
                {
                    return 0;
                }
                if (true.Equals(value))
                {
                    return 1;
                }
                return Convert.ToInt32(value);
            }
            catch
            {
                throw new ArgumentException(string.Format("\"{0}\" converted to type int fail.", value));
            }
        }

        /// <summary>
        /// 将对象转换成短整型值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static short ToShort(this object value)
        {
            try
            {
                return Convert.ToInt16(value.IsNullOrDbNull() ? 0 : value);
            }
            catch
            {
                throw new ArgumentException(string.Format("\"{0}\" converted to type short fail.", value));
            }
        }

        /// <summary>
        /// 将对象转换成双精度值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double ToDouble(this object value)
        {
            try
            {
                return Convert.ToDouble(value.IsNullOrDbNull() ? 0 : value);
            }
            catch
            {
                throw new ArgumentException(string.Format("\"{0}\" converted to type double fail.", value));
            }
        }
        /// <summary>
        /// 将对象转换成单精度值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static decimal ToDecimal(this object value)
        {
            try
            {
                return Convert.ToDecimal(value.IsNullOrDbNull() ? 0 : value);
            }
            catch
            {
                throw new ArgumentException(string.Format("\"{0}\" converted to type decimal fail.", value));
            }
        }
        /// <summary>
        /// 将对象转换成单精度浮点值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static float ToFloat(this object value)
        {
            try
            {
                return Convert.ToSingle(value.IsNullOrDbNull() ? 0 : value);
            }
            catch
            {
                throw new ArgumentException(string.Format("\"{0}\" converted to type decimal fail.", value));
            }
        }
        /// <summary>
        /// 将对象转换成布尔值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool ToBool(this object value)
        {
            try
            {
                if (value.IsNullOrDbNull() || "0".Equals(value))
                {
                    return false;
                }
                if ("1".Equals(value))
                {
                    return true;
                }
                return Convert.ToBoolean(value);
            }
            catch
            {
                throw new ArgumentException(string.Format("\"{0}\" converted to type bool fail.", value));
            }
        }
        /// <summary>
        /// 将对象转换成字节值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte ToByte(this object value)
        {
            try
            {
                return Convert.ToByte(value.IsNullOrDbNull() ? 0 : value);
            }
            catch
            {
                throw new ArgumentException(string.Format("\"{0}\" converted to type byte fail.", value));
            }
        }

        /// <summary>
        /// 将对象转换成64位无符号
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static UInt64 ToUInt64(this object value)
        {
            try
            {
                return Convert.ToUInt64(value.IsNullOrDbNull() ? 0 : value);
            }
            catch
            {
                throw new ArgumentException(string.Format("\"{0}\" converted to type UInt64 fail.", value));
            }
        }
        /// <summary>
        /// 将对象转换成32位无符号
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static UInt32 ToUInt32(this object value)
        {
            try
            {
                return Convert.ToUInt32(value.IsNullOrDbNull() ? 0 : value);
            }
            catch
            {
                throw new ArgumentException(string.Format("\"{0}\" converted to type ToUInt32 fail.", value));
            }
        }
        /// <summary>
        /// 将对象转换成16位无符号
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static UInt16 ToUInt16(this object value)
        {
            try
            {
                return Convert.ToUInt16(value.IsNullOrDbNull() ? 0 : value);
            }
            catch
            {
                throw new ArgumentException(string.Format("\"{0}\" converted to type ToUInt16 fail.", value));
            }
        }
        /// <summary>
        /// 将对象转换成时间值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this object value)
        {
            return ToDateTime(value, SqlMinDate);
        }

        /// <summary>
        /// 将对象转换成时间值
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this object value, DateTime defValue)
        {
            try
            {
                if (value.IsNullOrDbNull()) return defValue;
                return Convert.ToDateTime(value);
            }
            catch
            {
                throw new ArgumentException(string.Format("\"{0}\" converted to type datetime fail.", value));
            }
        }

        /// <summary>
        /// 将对象转换成枚举值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T ToEnum<T>(this object value)
        {
            return (T)ToEnum(value, typeof(T));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object ToEnum(this object value, Type type)
        {
            try
            {
                if (value is string)
                {
                    string tempValue = value.ToNotNullString();
                    return tempValue.IsEmpty() ? 0 : Enum.Parse(type, tempValue, true);
                }
                return Enum.ToObject(type, (value == null || value == DBNull.Value) ? 0 : value);
            }
            catch
            {
                throw new ArgumentException(string.Format("\"{0}\" converted to Enum {1} fail.", value, type.Name));
            }
        }

        #endregion
    }
}
