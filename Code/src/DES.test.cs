using System.Text;
using Xunit;

namespace DES_Algorithm
{
    public class DES_Test
    {

    #region >>> DES.Hash() <<<
        [Theory]
        [InlineData("CRYPTOGRAPHYOSA", "ROSHARAN")]
        [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ", "ROSHARAN")]
        [InlineData("CRYPTOGRAPHYOSA", "CONVENIENCE")]
        [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ", "CONVENIENCE")]
        public void Hash_TwoWayEncryption(string input, string key)
        {
            // Arrange:
            // var key_bytes = Encoding.UTF8.GetBytes(key);
            // Act:
            string output = DES.Decrypt(DES.Encrypt(input, key: key), key: key);
            // Assert:
            string output_normalized = output;
            Assert.Equal(input, output_normalized);
        }

        // [Theory]
        // [InlineData("0123456789ABCDEF", "133457799BBCDFF1", "i7R6DPCpYm3UC7SxbTuGF/j3FKwgfj76")]
        // public void Hash_Encryption(string input, string key, string expectedOutput)
        // {
        //     // Arrange:
        //     var key_bytes = Encoding.UTF8.GetBytes(key);
        //     // Act:
        //     var output = DES.Encrypt(input, key: key);
        //     // Assert:
        //     string output_normalized = Encoding.UTF8.GetString(output);
        //     Assert.Equal(expectedOutput, output_normalized);
        // }

        // [Theory]
        // [InlineData("CRYPTOGRAPHYOSA", "ROSHARAN")]
        // [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ", "ROSHARAN")]
        // [InlineData("CRYPTOGRAPHYOSA", "CONVENIENCE")]
        // [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ", "CONVENIENCE")]
        // public void Hash_Decryption(string input, string key, string expectedOutput)
        // {
        //     // Arrange:
        //     var key_bytes = Encoding.UTF8.GetBytes(key);
        //     // Act:
        //     byte[] output = DES_Algorithm.Decrypt(fullKey: key_bytes, input);
        //     // Assert:
        //     string output_normalized = Encoding.UTF8.GetString(output);
        //     Assert.Equal(expectedOutput, output_normalized);
        // }
    #endregion >>> DES_Algorithm.Hash() <<<

    }
}