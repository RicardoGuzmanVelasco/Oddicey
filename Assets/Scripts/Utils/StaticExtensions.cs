using System;

namespace Utils.StaticExtensions
{
    public static class TypeExtensions
    {
        /// <summary>
        /// Finds the declaring type of a variable.
        /// </summary>
        public static Type GetStaticType<T>(T x)
        {
            return typeof(T);
        }
    }
}