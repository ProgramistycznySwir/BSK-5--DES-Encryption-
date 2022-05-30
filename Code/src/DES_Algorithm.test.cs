using System.Text;
using Xunit;

namespace DES_Algorithm
{
    public class DES_Algorithm_Test
    {

    #region >>> DES_Algorithm.Hash() <<<
        [Theory]
        [InlineData("CRYPTOGRAPHYOSA", "ROSHARAN")]
        [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ", "ROSHARAN")]
        [InlineData("CRYPTOGRAPHYOSA", "CONVENIENCE")]
        [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ", "CONVENIENCE")]
        public void Hash_TwoWayEncryption(string input, string key)
        {
            // Arrange:
            var key_bytes = Encoding.UTF8.GetBytes(key);
            // Act:
            string output = DES_Algorithm.Decrypt(fullKey: key_bytes, DES_Algorithm.Encrypt(fullKey: key_bytes, input));
            // Assert:
            string output_normalized = output;
            Assert.Equal(input, output_normalized);
        }

        [Theory]
        [InlineData("0123456789ABCDEF", "133457799BBCDFF1", "85E813540F0AB405")]
        public void Hash_Encryption(string input, string key, string expectedOutput)
        {
            // Arrange:
            var key_bytes = Encoding.UTF8.GetBytes(key);
            // Act:
            string output = DES_Algorithm.Encrypt(fullKey: key_bytes, input);
            // Assert:
            // string output_normalized = Encoding.UTF8.GetString(output);
            Assert.Equal(expectedOutput, output);
        }

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