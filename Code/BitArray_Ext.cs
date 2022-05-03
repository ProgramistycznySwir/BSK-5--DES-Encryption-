using System.Collections;

namespace DES_Algorithm
{
    public static class BitArray_Ext
    {
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
        

        // Has to be immutable.
        public static BitArray CycleShiftLeft(this BitArray self)
        {
            // 1000 <<1 => 0001  0000
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
    }
}