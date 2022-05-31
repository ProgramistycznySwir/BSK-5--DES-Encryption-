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
            // Arrange: Nothing.
            // Act:
            string output = DES.Decrypt(DES.Encrypt(input, key: key), key: key);
            // Assert:
            string output_normalized = output;
            Assert.Equal(input, output_normalized);
        }
        #endregion >>> DES_Algorithm.Hash() <<<

    }
}