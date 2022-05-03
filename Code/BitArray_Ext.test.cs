using System.Collections;
using Xunit;

namespace DES_Algorithm
{
    public class BitArray_Ext_Test
    {
        // [Theory]
        // [InlineData("CRYPTOGRAPHY", "CTARPORPYYGH", 3)]
        // [InlineData("ABCDEFGHIJKLMNOPRSTUWVXYZ", "AEIMRWZBDFHJLNPSUVYCGKOTX", 3)]
        // public void WordEncryption(string word, string encryptedWord_expected, int railCount)
        // {
        //     // Arrange:
        //     RailFence encryptor = new(railCount: railCount);
        //     // Act:
        //     string encryptedWord = encryptor.Encrypt(word);
        //     // Assert:
        //     Assert.Equal(encryptedWord_expected, encryptedWord);
        // }

        // [Theory]
        // [InlineData("CTARPORPYYGH", "CRYPTOGRAPHY", 3)]
        // [InlineData("AEIMRWZBDFHJLNPSUVYCGKOTX", "ABCDEFGHIJKLMNOPRSTUWVXYZ", 3)]
        // public void WordDecryption(string word, string decryptedWord_expected, int railCount)
        // {
        //     // Arrange:
        //     RailFence encryptor = new(railCount: railCount);
        //     // Act:
        //     string decryptedWord = encryptor.Decrypt(word);
        //     // Assert:
        //     Assert.Equal(decryptedWord_expected, decryptedWord);
        // }

        [Fact]
        public void Split_ShouldWork()
        {
            // Arrange:
            BitArray toSplit = new BitArray(new byte[]{0b11111111, 0b00000000});
            // Act:
            var(left, right) = toSplit.Split();
            // Assert:
            var L_expected = new BitArray(new byte[]{0b11111111});
            Assert.Equal(L_expected, left);
            var R_expected = new BitArray(new byte[]{0b00000000});
            Assert.Equal(R_expected, right);
        }


        [Fact]
        public void Unite_ShouldWork()
        {
            // Arrange:
            BitArray L = new BitArray(new byte[]{0b11111111, 0b00000000});
            BitArray R = new BitArray(new byte[]{0b11111111, 0b00000000});
            // Act:
            var result = BitArray_Ext.Unite(L, R);
            // Assert:
            var expected = new BitArray(new byte[]{0b11111111, 0b00000000, 0b11111111, 0b00000000});
            Assert.Equal(expected, result);
        }

        // public static (BitArray L, BitArray R) Split(this BitArray self)
        // {
        //     var lenght = self.Length;
        //     var half = lenght / 2;
        //     var(L, R) = (new BitArray(half), new BitArray(half));
        //     for(int i = 0; i < half; i++){
        //         L[i] = self[i];
        //     }
        //     for(int i = half; i < 2*half; i++){
        //         R[i] = self[i];
        //     }

        //     return (L, R);

        //     // for(int i = 0; i < lenght; i++)
        //     //     (i < half ? L : R)[i % half] = self[i];
        // }

        // public static BitArray Unite(BitArray L, BitArray R){
        //     var length = L.Length + R.Length;
        //     BitArray result = new BitArray( length );
        //     for(int i = 0; i < length/2; i++){
        //         result[i] = L[i];
        //     }
        //     for(int i = length; i < length; i++){
        //         result[i] = R[i];
        //     }

        //     return result;

        // }

        [Fact]
        public void CycleShiftLeft_ShouldConvert128To1()
        {
            // Arrange:
            var input = new BitArray(new byte[] { 0b1000_0000 });
            // Act:
            //var result = BitArray_Ext.CycleShiftLeft(input);
            var result = input.CycleShiftLeft();
            // Assert:
            var expected = new BitArray(new byte[]{0b0000_0001 });
            Assert.Equal(expected, result);
        }
        // // Has to be immutable.
        // public static BitArray CycleShiftLeft(this BitArray self)
        // {
        //     // 1000 <<1 => 0001  0000
        //     var length = self.Length;
        //     bool temp;
        //     for(int i = 0; i < length - 1; i++)
        //     {
        //         temp = self[0];
        //         self[0] = self[i + 1];
        //         self[i +1] = temp;
        //     }
        //     return self;
        // }
    }
}