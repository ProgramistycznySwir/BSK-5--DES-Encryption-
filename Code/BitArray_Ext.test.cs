using System.Collections;
using Xunit;

namespace BitArray_Extensions
{
    public class BitArray_Ext_Test
    {

    #region >>> BitArray_Ext.Split() <<<
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
    #endregion >>> BitArray_Ext.Split() <<<

    #region >>> BitArray_Ext.Unite() <<<
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
    #endregion >>> BitArray_Ext.Unite() <<<

    #region >>> BitArray_Ext.CycleShiftLeft() <<<
        [Fact]
        public void CycleShiftLeft_ShouldConvert128To1()
        {
            // Arrange:
            var input = new BitArray(new byte[] { 0b1000_0000 });
            // Act:
            var result = input.CycleShiftLeft();
            // Assert:
            var expected = new BitArray(new byte[]{0b0000_0001 });
            Assert.Equal(expected, result);
        }
        [Fact]
        public void CycleShiftLeft_ShouldBeImmutable()
        {
            // Arrange:
            var input = new BitArray(new byte[] { 0b1000_0000 });
            // Act:
            var result = input.CycleShiftLeft();
            // Assert:
            var expected = new BitArray(new byte[] { 0b1000_0000 });
            Assert.Equal(expected, input);
        }
    #endregion >>> BitArray_Ext.CycleShiftLeft() <<<

    }
}