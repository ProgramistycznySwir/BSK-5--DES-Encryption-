using System.Collections;

namespace BitArray_Extensions
{
    public static partial class BitArray_Ext
    {
        public static (BitArray L, BitArray R) Split(this BitArray self)
        {
            var lenght = self.Length;
            var half = lenght / 2;
            var(L, R) = (new BitArray(half), new BitArray(half));
            for(int i = 0; i < lenght; i++)
                (i < half ? L : R)[i % half] = self[i];

            return (L, R);
        }

        public static BitArray Unite(BitArray L, BitArray R){
            var length = L.Length + R.Length;
            BitArray result = new BitArray( length );
            for(int i = 0; i < L.Length; i++){
                result[i] = L[i];
                result[L.Length + i] = R[i];
            }
            
            return result;
        }
        public static void Rotate(this BitArray self, byte by)
        {
            var length = self.Length;
            bool temp;
            for(int times = by; times > 0; times--)
                for(int i = 0; i < length - 1; i++)
                {
                    temp = self[0];
                    self[0] = self[i + 1];
                    self[i + 1] = temp;
                }
        }

        // Optimised way of copying BitArrays.
        public static void CopyTo(this BitArray self, ref BitArray copy)
        {
            for(int i = 0; i < self.Length; i++)
                copy[i] = self[i];
        }
    }
}