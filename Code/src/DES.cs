using System.Collections;
using System.Text;
using BitArray_Extensions;
using static DES_Algorithm.DES_Tables;

namespace DES_Algorithm;


public static class DES {
    const int KeyLengthInBytes = 8;

    public static byte[] Encrypt(string message, string key)
        => Hash(Encoding.UTF8.GetBytes(message), GetKeyBytes(key), true);

    public static string Decrypt(ReadOnlySpan<byte> encryptedMessage, string key)
        => Encoding.UTF8.GetString(Hash(encryptedMessage, GetKeyBytes(key), false)).TrimEnd('\0');

    private static byte[] NormalizeMessage(ReadOnlySpan<byte> originalMessage)
    {
        //TODO (optimize): Later make it more optimized by just padding the last block.
        var length = originalMessage.Length;
        if (length % 8 is 0)
            return (byte[])originalMessage.ToArray();
        
        var normalizedMessage = new byte[length + 8 - (length % 8)];
        originalMessage.CopyTo(normalizedMessage);
        return normalizedMessage;
    }

    private static byte[] Hash(ReadOnlySpan<byte> originalMessage, byte[] keyBytes, bool encrypt)
    {
        var messageBytes = NormalizeMessage(originalMessage);
        var subKeys = CreateSubKeys(keyBytes);

        var blockCount = messageBytes.Length / 8;
        Task.WaitAll(
            Enumerable.Range(0, blockCount)
                .Select(i => Task.Run(() => HashBlock(messageBytes.AsSpan().Slice(i * 8, 8), subKeys, encrypt)))
                .ToArray());

        return messageBytes;
    }

    private static void HashBlock(Span<byte> messageBytes, IReadOnlyList<BitArray> subKeys, bool encrypt)
    {
        messageBytes.Reverse();
        var messageBits = new BitArray(messageBytes.ToArray());

        var left  = new BitArray(32);
        var right = new BitArray(32);
        for (var i = 0; i < 32; i++)
        {
            left[31 - i]  = messageBits[64 - IP[i]];
            right[31 - i] = messageBits[64 - IP[i + 32]];
        }
        // TODO: Replace with split.
        // var(left, right) = Permutate(messageBits, IP, 64).Split();

        var temp = new BitArray(32);
        // Iterating over 16 keys:
        for (var i = 0; i < 16; i++)
        {
            right.CopyTo(ref temp);

            var subKey = encrypt ? subKeys[i] : subKeys[15 - i];
            F(right, subKey);
            right = right.Xor(left);

            (left, temp) = (temp, left);
        }

        // Reunifying block.
        var leftRight = BitArray_Ext.Unite(left, right);

        // Applying last permutation of algorithm.
        PermutateInto(ref messageBits, leftRight, FP);

        // Converting result and applying it to span.
        var tempByteArray = new byte[8];
        messageBits.CopyTo(tempByteArray, 0);
        tempByteArray.CopyTo(messageBytes);
        messageBytes.Reverse();
    }

    private static void F(BitArray right, BitArray subKey) {
        var result = Permutate(right, EP, 48).Xor(subKey); // K[n] + E(R[n-1])

        var newRight = new BitArray(32);
        // Hashing newRight.
        for (var j = 0; j < 8; j++) {
            var pack = j * 6;

            byte row = 0;
            row |= Convert.ToByte(result[pack]);
            row |= (byte)(Convert.ToInt32(result[pack + 5]) << 1);

            byte column = 0;
            for (var k = 0; k < 4; k++)
                column |= (byte)(Convert.ToInt32(result[pack + k + 1]) << k);

            var value = SBox[7 - j, row, column];

            for (var k = 0; k < 4; k++)
                newRight[j * 4 + k] = Convert.ToBoolean(value >> k & 1);
        }
        // Final permutation in F function.
        PermutateInto(ref right, newRight, P);
    }
    private static BitArray[] CreateSubKeys(byte[] keyBytes)
    {
        Array.Reverse(keyBytes);
        var keyBits = new BitArray(keyBytes);

        var left = Permutate(keyBits, K1P, 28);
        var right = Permutate(keyBits, K2P, 28);

        var subKeys = new BitArray[16];
        var temp = new BitArray(56);

        for (var i = 0; i < 16; i++)
        {
            left.Rotate(ShiftBits[i]);
            right.Rotate(ShiftBits[i]);

            temp = BitArray_Ext.Unite(right, left);

            subKeys[i] = Permutate(temp, CP, 48);
        }

        return subKeys;
    }

    private static byte[] GetKeyBytes(string key)
    {
        byte[] keyBytes  = new byte[KeyLengthInBytes];
        Encoding.UTF8.GetBytes(key).AsSpan()[..8].CopyTo(keyBytes);
        return keyBytes;
    }

    /// <summary>
    /// Mutable version of Permutate
    /// </summary>
    private static void PermutateInto(ref BitArray result, BitArray @base, byte[] matrix)
    {
        int resultLenght = result.Length;
        int baseLenght = @base.Length;
        for(int i = 0; i < resultLenght; i++)
            result[resultLenght-1 - i] = @base[baseLenght - matrix[i]];
    }
    private static BitArray Permutate(BitArray @base, byte[] matrix, int resultLenght)
    {
        BitArray result = new(resultLenght);
        int baseLenght = @base.Length;
        for(int i = 0; i < resultLenght; i++)
            result[resultLenght-1 - i] = @base[baseLenght - matrix[i]];
        return result;
    }
}