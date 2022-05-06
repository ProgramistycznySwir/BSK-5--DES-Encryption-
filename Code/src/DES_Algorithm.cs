using System.Collections;
using Array_Extenstions;
using BitArray_Extensions;


namespace DES_Algorithm;


public static partial class DES_Algorithm
{
    
    // Wrappers:
    public static byte[] Encrypt(byte[] fullKey, string text)
        => Encrypt(fullKey, System.Text.Encoding.UTF8.GetBytes(text));
    public static byte[] Encrypt(byte[] fullKey, byte[] input)
        => Hash(fullKey, input, inverse: false);

    public static byte[] Decrypt(byte[] fullKey, string text)
        => Decrypt(fullKey, System.Text.Encoding.UTF8.GetBytes(text));
    public static byte[] Decrypt(byte[] fullKey, byte[] input)
        => Hash(fullKey, input, inverse: true);


    public static byte[] Hash(byte[] key, byte[] input, bool inverse= false)
    {
        byte[] result = NormalizeInput(input);

        // Step 1:
        BitArray[] subKeys = GenerateSubKeys(key);

        // Step 2:
        // We can highly parallelize this algorithm with use of tasks.
        // Also we can avoid unnecessary allocations with giving each task direct access to input memory, note that this
        //  would make this algorithm mutable, but i copy input in it's normalization.

        int blockCount = result.Length % 8;
        List<Task> tasks = new(blockCount);
        for (var i = 0; i < blockCount; i++) {
            int k = i;
            tasks[i] = Task.Run(() => HashBlock(subKeys, result.AsSpan().Slice(k * 8, 8), inverse));
        }
        Task.WaitAll(tasks.ToArray());

        // Autism:
        // Task.WaitAll(Enumerable.Range(0, blockCount).Select(i => Task.Run(() => HashBlock(subKeys, input.AsSpan().Slice(i * 8, 8), inverse))).ToArray());

        return result;
    }

    #region >>> Key permutation tables <<<
    public static readonly byte[] PC_1 = {
        57, 49, 41, 33, 25, 17, 9,
        1,  58, 50, 42, 34, 26, 18,
        10, 2,  59, 51, 43, 35, 27,
        19, 11, 3,  60, 52, 44, 36,
        63, 55, 47, 39, 31, 23, 15,
        7,  62, 54, 46, 38, 30, 22,
        14, 6,  61, 53, 45, 37, 29,
        21, 13, 5,  28, 20, 12, 4,
    };
    public static readonly byte[] PC_2 = {
        14, 17, 11, 24,  1,  5,
         3, 28, 15,  6, 21, 10,
        23, 19, 12,  4, 26,  8,
        16,  7, 27, 20, 13,  2,
        41, 52, 31, 37, 47, 55,
        30, 40, 51, 45, 33, 48,
        44, 49, 39, 56, 34, 53,
        46, 42, 50, 36, 29, 32,
    };
    #endregion >>> Key permutation tables <<<
    private static BitArray[] GenerateSubKeys(byte[] key)
    {
        // Key Permutation I:
        byte[] key_normalized_ = new byte[8];
        key.AsSpan(0, 8).CopyTo(key_normalized_);
        BitArray key_normalized_bitArray_ = new BitArray(key_normalized_);
        
        var key_reduced_ = new BitArray(56);
        for(int i = 0; i < 56; i++)
            key_reduced_[i] = key_normalized_bitArray_[PC_1[i]];
        
        var(key_L, key_R) = key_reduced_.Split();

        // Create sub-keys:
        List<BitArray> key_L_Blocks = new(16);
        List<BitArray> key_R_Blocks = new(16);

        key_L_Blocks.Add(new BitArray(key_L).CycleShiftLeft());
        key_R_Blocks.Add(new BitArray(key_R).CycleShiftLeft());
        for(int i = 1; i < 16; i++)
        {
            key_L_Blocks.Add(new BitArray(key_L_Blocks[i-1]).CycleShiftLeft());
            key_R_Blocks.Add(new BitArray(key_R_Blocks[i-1]).CycleShiftLeft());
        }

        var key_Blocks = key_L_Blocks.Zip(key_R_Blocks).Select(e => BitArray_Ext.Unite(e.First, e.Second)).ToList();

        // Key Permutation II:
        var key2_Blocks = Enumerable.Range(0, 16).Select(e => new BitArray(48)).ToList();
        for(int i = 0; i < 16; i++)
            for(int ii = 0; ii < 48; ii++)
                key2_Blocks[i][ii] = key_Blocks[i][PC_2[ii]-1];

        return key2_Blocks.ToArray();
    }

    /// <summary>
    /// Pads input with 0s to make it's lenght the multiple of 64 bits.
    /// </summary>
    private static byte[] NormalizeInput(byte[] input)
    {
        //TODO (optimize): Later make it more optimized by just padding the last block.
        var length = input.Length;
        if (length % 8 is 0)
            return (byte[])input.Clone();

        var normalizedInput = new byte[length + 8 - (length % 8)];
        input.AsSpan().CopyTo(normalizedInput);
        return normalizedInput;
    }


#region >>> Constants 2 <<<
    public static readonly byte[] IP = {
        58, 50, 42, 34, 26, 18, 10, 2,
        60, 52, 44, 36, 28, 20, 12, 4,
        62, 54, 46, 38, 30, 22, 14, 6,
        64, 56, 48, 40, 32, 24, 16, 8,
        57, 49, 41, 33, 25, 17,  9, 1,
        59, 51, 43, 35, 27, 19, 11, 3,
        61, 53, 45, 37, 29, 21, 13, 5,
        63, 55, 47, 39, 31, 23, 15, 7,
    };
    public static readonly byte[] E = {
        32,  1,  2,  3,  4,  5,
         4,  5,  6,  7,  8,  9,
         8,  9, 10, 11, 12, 13,
        12, 13, 14, 15, 16, 17,
        16, 17, 18, 19, 20, 21,
        20, 21, 22, 23, 24, 25,
        24, 25, 26, 27, 28, 29,
        28, 29, 30, 31, 32,  1,
    };
#endregion >>> Constants 2 <<<


    private static BitArray HashBlock(BitArray[] keys, Span<byte> input, bool inverse = false)
    {
        var input_bitArray = new BitArray(input.ToArray());
        var input_IP = new BitArray(64);
        for(int i = 0; i < 64; i++)
            input_IP[i] = input_bitArray[IP[i]-1];

        var(input_L, input_R) = input_IP.Split();

        var temp = new BitArray(32);
        for(int i = 0; i < 16; i++)
        {
            //TODO: Prolly wrong.
            // L[n] = R[n-1]
            // R[n] = L[n-1] XOR f(R[n-1], key[n])
            input_L.CopyTo(temp);
            input_R.CopyTo(input_L);

            var key = keys[inverse ? 15 - i : i];
            input_R = temp.Xor(F(key, input_L));

            BitArray F(BitArray key, BitArray R)
            {
                var R_expanded = new BitArray(48);
                for(int i = 0; i < 48; i++)
                    R_expanded[i] = R[E[i]-1];
                return key.Xor(R_expanded);
            }
        }

        BitArray_Ext.UniteInto(input_L, input_R, input_bitArray);

        // var thingie = InitPermutBlock(input);



        throw new NotImplementedException();
    }
}
