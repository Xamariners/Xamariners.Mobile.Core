using System;

namespace Xamariners.Mobile.Core.Extensions
{
    public static class TypesExtension
    {
        /// <summary>
        /// Determines whether [is integer type].
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        ///   <c>true</c> if [is integer type] [the specified type]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsIntegerType(this Type type)
        {
            type = Nullable.GetUnderlyingType(type) ?? type;
            if (type == typeof(long)
                || type == typeof(ulong)
                || type == typeof(int)
                || type == typeof(uint)
                || type == typeof(short)
                || type == typeof(ushort)
                || type == typeof(byte)
                || type == typeof(sbyte)
                || type == typeof(System.Numerics.BigInteger))
                return true;
            return false;
        }
    }
}
