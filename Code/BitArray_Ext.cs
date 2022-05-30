using System.Collections;

namespace BitArray_Extensions
{
    public static class BitArray_Ext
    {
        public static byte[] ToByteArray(this BitArray self)
        {
            if(self.Count % 8 is not 0)
                throw new ArgumentException("Input BitArray's lenght should be divisible by 8!");
            int lenght = self.Count / 8;
            byte[] result = new byte[lenght];
            for(int i = 0; i < lenght; i++)
                for(int ii = 0; ii < 8; ii++)
                    result[i] += (byte)(Convert.ToByte(self[i*8 + ii]) << (ii));
            return result;
        }

        public static (BitArray L, BitArray R) Split(this BitArray self)
        {
            var lenght = self.Length;
            var half = lenght / 2;
            var(L, R) = (new BitArray(half), new BitArray(half));
            for(int i = 0; i < half; i++){
                L[i] = self[i];
            }
            for(int i = half; i < 2*half; i++){
                R[i-half] = self[i];
            }
    
            return (L, R);

            // for(int i = 0; i < lenght; i++)
            //     (i < half ? L : R)[i % half] = self[i];
        }

        public static BitArray Unite(BitArray L, BitArray R){
            var length = L.Length + R.Length;
            BitArray result = new BitArray( length );
            for(int i = 0; i < L.Length; i++){
                result[i] = L[i];
            }
            for(int i = L.Length; i < length; i++){
                result[i] = R[i-L.Length];
            }

            return result;
        }
        /// <summary>
        /// Make sure on your own if out.Lenght == L.Lenght + R.Lenght!!!
        /// </summary>
        // public static void UniteInto(BitArray L, BitArray R, BitArray @out){
        //     for(int i = 0; i < L.Length; i++){
        //         @out[i] = L[i];
        //         @out[L.Length + i] = R[i-L.Length];
        //     }
        // }
        

        // Has to be immutable.
        public static BitArray CycleShiftLeft(this BitArray self)
        {
            // 1000 <<1 => 0001  0000
            self = new BitArray(self);
            var length = self.Length;
            bool temp;
            for(int i = 0; i < length - 1; i++)
            {
                temp = self[0];
                self[0] = self[i + 1];
                self[i +1] = temp;
            }
            return self;
        }

        // Optimised way of copying BitArrays.
        public static void CopyTo(this BitArray self, BitArray copy)
        {
            for(int i = 0; i < self.Length; i++)
                copy[i] = self[i];
        }
    }
}