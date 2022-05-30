using System.Collections;
using Array_Extenstions;
using BitArray_Extensions;
using static System.Text.Encoding;


namespace DES_Algorithm;


public static partial class DES_Algorithm
{
    
    // Wrappers:
    public static string Encrypt(byte[] fullKey, string text)
        => System.Convert.ToBase64String(Encrypt(fullKey, System.Text.Encoding.UTF8.GetBytes(text)));
    public static byte[] Encrypt(byte[] fullKey, byte[] input)
        => Hash(fullKey, input, inverse: false);

    public static string Decrypt(byte[] fullKey, string text)
        => UTF8.GetString(Decrypt(fullKey, System.Convert.FromBase64String(text))).TrimEnd('\0');
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
    private static readonly byte[] PC_1 = {
        57, 49, 41, 33, 25, 17, 9,
        1,  58, 50, 42, 34, 26, 18,
        10, 2,  59, 51, 43, 35, 27,
        19, 11, 3,  60, 52, 44, 36,
        63, 55, 47, 39, 31, 23, 15,
        7,  62, 54, 46, 38, 30, 22,
        14, 6,  61, 53, 45, 37, 29,
        21, 13, 5,  28, 20, 12, 4,
    };
    private static readonly byte[] PC_2 = {
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
    private static readonly byte[] IP = {
        58, 50, 42, 34, 26, 18, 10, 2,
        60, 52, 44, 36, 28, 20, 12, 4,
        62, 54, 46, 38, 30, 22, 14, 6,
        64, 56, 48, 40, 32, 24, 16, 8,
        57, 49, 41, 33, 25, 17,  9, 1,
        59, 51, 43, 35, 27, 19, 11, 3,
        61, 53, 45, 37, 29, 21, 13, 5,
        63, 55, 47, 39, 31, 23, 15, 7,
    };
    private static readonly byte[] E = {
        32,  1,  2,  3,  4,  5,
         4,  5,  6,  7,  8,  9,
         8,  9, 10, 11, 12, 13,
        12, 13, 14, 15, 16, 17,
        16, 17, 18, 19, 20, 21,
        20, 21, 22, 23, 24, 25,
        24, 25, 26, 27, 28, 29,
        28, 29, 30, 31, 32,  1,
    };
    private static readonly byte[,,] SBox = {
        {
            { 14, 4, 13, 1, 2, 15, 11, 8, 3, 10, 6, 12, 5, 9, 0, 7 },
            { 0, 15, 7, 4, 14, 2, 13, 1, 10, 6, 12, 11, 9, 5, 3, 8 },
            { 4, 1, 14, 8, 13, 6, 2, 11, 15, 12, 9, 7, 3, 10, 5, 0 },
            { 15, 12, 8, 2, 4, 9, 1, 7, 5, 11, 3, 14, 10, 0, 6, 13 }
        }, {
            { 15, 1, 8, 14, 6, 11, 3, 4, 9, 7, 2, 13, 12, 0, 5, 10 },
            { 3, 13, 4, 7, 15, 2, 8, 14, 12, 0, 1, 10, 6, 9, 11, 5 },
            { 0, 14, 7, 11, 10, 4, 13, 1, 5, 8, 12, 6, 9, 3, 2, 15 },
            { 13, 8, 10, 1, 3, 15, 4, 2, 11, 6, 7, 12, 0, 5, 14, 9 }
        }, {
            { 10, 0, 9, 14, 6, 3, 15, 5, 1, 13, 12, 7, 11, 4, 2, 8 },
            { 13, 7, 0, 9, 3, 4, 6, 10, 2, 8, 5, 14, 12, 11, 15, 1 },
            { 13, 6, 4, 9, 8, 15, 3, 0, 11, 1, 2, 12, 5, 10, 14, 7 },
            { 1, 10, 13, 0, 6, 9, 8, 7, 4, 15, 14, 3, 11, 5, 2, 12 }
        }, {
            { 7, 13, 14, 3, 0, 6, 9, 10, 1, 2, 8, 5, 11, 12, 4, 15 },
            { 13, 8, 11, 5, 6, 15, 0, 3, 4, 7, 2, 12, 1, 10, 14, 9 },
            { 10, 6, 9, 0, 12, 11, 7, 13, 15, 1, 3, 14, 5, 2, 8, 4 },
            { 3, 15, 0, 6, 10, 1, 13, 8, 9, 4, 5, 11, 12, 7, 2, 14 }
        }, {
            { 2, 12, 4, 1, 7, 10, 11, 6, 8, 5, 3, 15, 13, 0, 14, 9 },
            { 14, 11, 2, 12, 4, 7, 13, 1, 5, 0, 15, 10, 3, 9, 8, 6 },
            { 4, 2, 1, 11, 10, 13, 7, 8, 15, 9, 12, 5, 6, 3, 0, 14 },
            { 11, 8, 12, 7, 1, 14, 2, 13, 6, 15, 0, 9, 10, 4, 5, 3 }
        }, {
            { 12, 1, 10, 15, 9, 2, 6, 8, 0, 13, 3, 4, 14, 7, 5, 11 },
            { 10, 15, 4, 2, 7, 12, 9, 5, 6, 1, 13, 14, 0, 11, 3, 8 },
            { 9, 14, 15, 5, 2, 8, 12, 3, 7, 0, 4, 10, 1, 13, 11, 6 },
            { 4, 3, 2, 12, 9, 5, 15, 10, 11, 14, 1, 7, 6, 0, 8, 13 }
        }, {
            { 4, 11, 2, 14, 15, 0, 8, 13, 3, 12, 9, 7, 5, 10, 6, 1 },
            { 13, 0, 11, 7, 4, 9, 1, 10, 14, 3, 5, 12, 2, 15, 8, 6 },
            { 1, 4, 11, 13, 12, 3, 7, 14, 10, 15, 6, 8, 0, 5, 9, 2 },
            { 6, 11, 13, 8, 1, 4, 10, 7, 9, 5, 0, 15, 14, 2, 3, 12 }
        }, {
            { 13, 2, 8, 4, 6, 15, 11, 1, 10, 9, 3, 14, 5, 0, 12, 7 },
            { 1, 15, 13, 8, 10, 3, 7, 4, 12, 5, 6, 11, 0, 14, 9, 2 },
            { 7, 11, 4, 1, 9, 12, 14, 2, 0, 6, 10, 13, 15, 3, 5, 8 },
            { 2, 1, 14, 7, 4, 10, 8, 13, 15, 12, 9, 0, 3, 5, 6, 11 }
        }
    };
    private static readonly byte[] P = {
        16, 7,  20, 21,
        29, 12, 28, 17,
        1,  15, 23, 26,
        5,  18, 31, 10,
        2,  8,  24, 14,
        32, 27, 3,  9,
        19, 13, 30, 6,
        22, 11, 4,  25
    };
    private static readonly byte[] IP_1 = {
        40, 8, 48, 16, 56, 24, 64, 32,
        39, 7, 47, 15, 55, 23, 63, 31,
        38, 6, 46, 14, 54, 22, 62, 30,
        37, 5, 45, 13, 53, 21, 61, 29,
        36, 4, 44, 12, 52, 20, 60, 28,
        35, 3, 43, 11, 51, 19, 59, 27,
        34, 2, 42, 10, 50, 18, 58, 26,
        33, 1, 41, 9, 49, 17, 57, 25
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
                    R_expanded[i] = R[E[i]-1]; // E(R[n-1])
                
                R_expanded = R_expanded.Xor(key); // K[n] + E(R[n-1])

                var result = new BitArray(32);
                for(int i = 0; i < 8; i++)
                {
                    int pack = i * 6;
                    // var box = SBox[i];
                    byte S_i = 0;
                        S_i |= System.Convert.ToByte(result[pack]);
                        S_i |= (byte)(System.Convert.ToInt32(result[pack + 5]) << 1);

                    byte S_j = 0;
                    for (var k = 0; k < 4; k++)
                        S_j |= (byte)(System.Convert.ToInt32(result[pack + k + 1]) << k);
                    
                    byte fromSBox = SBox[i, S_i, S_j];

                    for(int ii = 0; ii < 4; ii++)
                        result[i*4 + ii] = System.Convert.ToBoolean(fromSBox >> ii & 1);
                }

                var result_P = new BitArray(32);
                for(int i = 0; i < 32; i++)
                    result_P[i] = result[P[i]-1];

                return result_P;

                // return key.Xor(R_expanded);
            }
        }

        var RL = BitArray_Ext.Unite(input_R, input_L);
        var result = new BitArray(64);
        for(int i = 0; i < 64; i++)
            result[i] = RL[IP_1[i]-1];
        // BitArray_Ext.UniteInto(input_L, input_R, input_bitArray);

        // var thingie = InitPermutBlock(input);



        throw new NotImplementedException();
    }
}
