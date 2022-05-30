// See https://aka.ms/new-console-template for more information
// Console.WriteLine("Hello, World!");

using System.Text;

Action<object> Log = Console.WriteLine;

var rawResult = Code.src.DES.Encrypt("CRYPTOGRAPHYOSA", "ROSHARAN");
var rawResult_my = DES_Algorithm.DES_Algorithm.Encrypt(Encoding.UTF8.GetBytes("ROSHARAN"), "CRYPTOGRAPHYOSA");
// var result = Encoding.UTF8.GetString(rawResult);
var result = Convert.ToBase64String(rawResult);
var result_my = rawResult_my;
Log("w4xaxkb8sW/dbi7E/MgCAg==");
Log(result);
Log(result_my);
// Code.src.DES.Decrypt("CRYPTOGRAPHYOSA", "ROSHARAN");
