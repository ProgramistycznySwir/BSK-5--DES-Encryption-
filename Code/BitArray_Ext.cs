using System.Collections;

namespace BitArray_Extensions
{
    public static partial class BitArray_Ext
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
            // for(int i = 0; i < half; i++){
            //     L[i] = self[i];
            // }
            // for(int i = half; i < lenght; i++){
            //     R[i-half] = self[i];
            // }
            for(int i = 0; i < lenght; i++)
                (i < half ? L : R)[i % half] = self[i];
    
            return (L, R);

        }
        public static (BitArray L, BitArray R) SplitReverse(this BitArray self)
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

        public static string ToBinaryString(this BitArray bitArray) {
            var byteArray = new byte[8];
            bitArray.CopyTo(byteArray, 0);
            return string.Join("", byteArray.Reverse().Select(b => Convert.ToString(b, 2).PadLeft(8, '0')))[(64 - bitArray.Length)..];
        }
    }
}