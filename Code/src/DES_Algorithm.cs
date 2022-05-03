


using System.Collections;

namespace DES_Algorithm;


public static partial class DES_Algorithm
{
    public static byte[,] PC_1 = {
        {57, 49, 41, 33, 25, 17, 9},
        {1, 58, 50, 42, 34, 26, 18},
        {10, 2, 59, 51, 43, 35, 27},
        {19, 11, 3, 60, 52, 44, 36},
        {63, 55, 47, 39, 31, 23, 15},
        {7, 62, 54, 46, 38, 30, 22},
        {14, 6, 61, 53, 45, 37, 29},
        {21, 13, 5, 28, 20, 12, 4},
    };
    public static readonly byte[] PC_1_flat = {
        57, 49, 41, 33, 25, 17, 9,
        1,  58, 50, 42, 34, 26, 18,
        10, 2,  59, 51, 43, 35, 27,
        19, 11, 3,  60, 52, 44, 36,
        63, 55, 47, 39, 31, 23, 15,
        7,  62, 54, 46, 38, 30, 22,
        14, 6,  61, 53, 45, 37, 29,
        21, 13, 5,  28, 20, 12, 4,
    };
    public static readonly byte[] PC_2_flat = {
        14, 17, 11, 24,  1,  5,
         3, 28, 15,  6, 21, 10,
        23, 19, 12,  4, 26,  8,
        16,  7, 27, 20, 13,  2,
        41, 52, 31, 37, 47, 55,
        30, 40, 51, 45, 33, 48,
        44, 49, 39, 56, 34, 53,
        46, 42, 50, 36, 29, 32,
    };
    
    

    public static byte[] Encrypt(BitArray fullKey, byte[] input)
    {
        // Key Permutation I:
        var key = new BitArray(56);
        for(int i = 0; i < 56; i++)
            key[i] = fullKey[PC_1_flat[i-1]];
        
        var(key_L, key_R) = key.Split();
        

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
                key2_Blocks[i][ii] = key_Blocks[i][PC_2_flat[ii]-1];

        // Step 2:


        throw new NotImplementedException();
    }


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


    public static BitArray EncryptBlock(List<BitArray> keys, BitArray input)
    {
        var input_IP = new BitArray(64);
        for(int i = 0; i < 64; i++)
            input_IP[i] = input[IP[i]-1];

        var(input_L, input_R) = input_IP.Split();

        for(int i = 0; i < 16; i++)
        {
            // L[n] = R[n-1]
            // R[n] = L[n-1] XOR f(R[n-1], key[n])
            var temp = new BitArray(input_L);
            input_L = new BitArray(input_R);
            // input_R = temp.Xor()
            // static BitArray 

            BitArray Func(BitArray key, BitArray R)
            {
                var R_expanded = new BitArray(48);
                for(int i = 0; i < 48; i++)
                    R_expanded[i] = R[E[i]-1];
                return key.Xor(R_expanded);
            }
        }

        // var thingie = InitPermutBlock(input);



        throw new NotImplementedException();


    }

    public static (BitArray Left, BitArray Right) InitPermutBlock(BitArray plainText)
    {
        throw new NotImplementedException();
        // return 
    }


    public static byte[] EncryptionBlockStep(byte[] input, byte[] key)
    {
        throw new NotImplementedException();
    }


    public static byte[] FinalPermutation(byte[] input)
    {
        throw new NotImplementedException();

    }
}
