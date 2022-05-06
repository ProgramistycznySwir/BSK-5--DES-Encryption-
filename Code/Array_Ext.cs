using System.Collections;

namespace Array_Extenstions
{
    public static class Array_Ext
    {
        public static T SafeGet<T>(this T[] self, int index)
            => self[ClampMod(index, self.Length)];

        /// <summary>
        /// Modulo function that returns value in range 0-[mod]
        /// </summary>
        private static int ClampMod(int value, int mod)
        {
            value %= mod;
            value += value < 0 ? mod : 0;
            return value;
        }
    }
}