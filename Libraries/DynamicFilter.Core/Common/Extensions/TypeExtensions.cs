using DynamicFilter.Core.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace DynamicFilter.Core.Common.Extensions
{
    public static class TypeExtensions
    {
        /// <summary>
        /// Return original non nullable type
        /// </summary>
        public static Type GetTypeUnderNullable(this Type type)
        {
            return type.IsNullable() ? Nullable.GetUnderlyingType(type) : type;
        }

        /// <summary>
        /// Check type is nullable
        /// </summary>
        public static bool IsNullable(this Type type)
        {
            if (type.IsGenericType)
                return type.GetGenericTypeDefinition() == typeof(Nullable<>);
            else
                return false;
        }
    }
}
