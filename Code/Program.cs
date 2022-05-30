// See https://aka.ms/new-console-template for more information
// Console.WriteLine("Hello, World!");

using System.Collections;
using System.Text;

Action<object> Log = Console.WriteLine;

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

var rawResult = Code.src.DES.Encrypt("CRYPTOGRAPHYOSA", "ROSHARAN");
var rawResult_my = DES_Algorithm.DES_Algorithm.Encrypt(Encoding.UTF8.GetBytes("ROSHARAN"), "CRYPTOGRAPHYOSA");
// var result = Encoding.UTF8.GetString(rawResult);
var result = Convert.ToBase64String(rawResult);
var result_my = rawResult_my;
Log("w4xaxkb8sW/dbi7E/MgCAg==");
Log(result);
Log(result_my);
// Code.src.DES.Decrypt("CRYPTOGRAPHYOSA", "ROSHARAN");
