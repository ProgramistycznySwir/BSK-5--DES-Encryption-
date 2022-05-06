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
            byte[] output = DES_Algorithm.Decrypt(fullKey: key_bytes, DES_Algorithm.Encrypt(fullKey: key_bytes, input));
            // Assert:
            string output_normalized = Encoding.UTF8.GetString(output);
            Assert.Equal(input, output_normalized);
        }
    #endregion >>> BitArray_Ext.Split() <<<

    }
}