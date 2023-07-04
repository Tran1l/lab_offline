using Nethereum.Signer;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using Nethereum.Hex.HexConvertors.Extensions;


namespace OfflineMachine
{
	class Program
	{
		private static Random random = new Random();

        public static string GetRandomString(int length)
        {
            const string chars = "0123456789abcdef";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        static void Main(string[] args)
		{
			String privateKeyHex = "0x" + GetRandomString(64);

			var privateKey = new EthECKey(privateKeyHex);
			
			byte[] publicKey = privateKey.GetPubKey();
			string publicKeyHex = "0x" + BitConverter.ToString(publicKey).Replace("-", "");
			
			byte[] publicKeyHash = SHA256.HashData(publicKey);
			String address = "0x" + publicKeyHash.ToHex().Substring(24);

			Console.WriteLine("Private Key: " + privateKeyHex);
			Console.WriteLine("Public Key: " + publicKeyHex);
			Console.WriteLine("Address: " + address);
			//Console.WriteLine("C:lab\\OnlineMachine\\bin\\Debug\\net5.0\\transaction_in.txt");

			StreamReader stream1 = new StreamReader(@"C:\Users\konop\Downloads\lab\OnlineMachine\bin\Debug\net5.0\transaction_in.txt");
			String from = address;
			String to = stream1.ReadLine();
			String nonce = stream1.ReadLine();
			String value = stream1.ReadLine();
			stream1.Close();

			Console.WriteLine(from);
			Console.WriteLine(to);
			Console.WriteLine(nonce);
			Console.WriteLine(value);

			var signer = new EthereumMessageSigner();
			String message = $"{from} {to} {nonce} {value}";
			String signature = signer.EncodeUTF8AndSign(message, new EthECKey(privateKeyHex));

			StreamWriter stream2 = new StreamWriter("./transaction_out.txt");
			stream2.WriteLine(signature);
			stream2.Close();
		}
	}
}
