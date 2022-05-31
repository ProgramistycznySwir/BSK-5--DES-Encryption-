// See https://aka.ms/new-console-template for more information
// Console.WriteLine("Hello, World!");

using System.Collections;
using System.Text;

Action<object> Log = Console.WriteLine;
void LogRed(string log) { Console.ForegroundColor = ConsoleColor.DarkRed; Console.WriteLine(log); Console.ResetColor(); }

goto Code;

// Console.Write("XOR:");
BitArray arr = new BitArray(new bool[] { true, false});
BitArray arr2 = new BitArray(new bool[] { false, true});
var temp = arr.Xor(arr2);

foreach(var thingie in arr)
	Console.Write(thingie);
Log("");
foreach(var thingie in temp)
	Console.Write(thingie);
Log("");
// PrintValues(arr.Xor(arr2), 8);


return;

Code: ;

// var rawResult = DES_Algorithm.DES.Encrypt("CRYPTOGRAPHYOSA", "ROSHARAN");
var rawResult = DES_Algorithm.DES.Encrypt("0123456789ABCDEF", "133457799BBCDFF1");
var rawResult_my = DES_Algorithm.DES_Algorithm.Encrypt(Encoding.UTF8.GetBytes("ROSHARAN"), "CRYPTOGRAPHYOSA");
// var result = Encoding.UTF8.GetString(rawResult);
var result = Convert.ToBase64String(rawResult);
// var result_my = Convert.ToBase64String(rawResult_my);
var result_my = rawResult_my;
Log("w4xaxkb8sW/dbi7E/MgCAg==");
LogRed(result);
Log("w4xaxkb8sW8A1Z+z5xlZNg==");
LogRed(result_my);
Log("");
var rawResultDecrypted_my = DES_Algorithm.DES_Algorithm.Decrypt(Encoding.UTF8.GetBytes("ROSHARAN"), rawResult_my);
var resultDecrypted_my = rawResultDecrypted_my;
Log(resultDecrypted_my);
Log("");
Log(DES_Algorithm.DES.Decrypt(rawResult, "ROSHARAN"));

// Code.src.DES.Decrypt("CRYPTOGRAPHYOSA", "ROSHARAN");
