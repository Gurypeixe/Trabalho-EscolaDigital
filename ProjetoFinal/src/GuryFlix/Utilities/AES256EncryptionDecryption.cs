using System;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

using Guryflix.Structures;

namespace Guryflix.Utilities
{
    class AES256EncryptionDecryption
    {
        byte[] key, iv;
        public AES256EncryptionDecryption(string password)
        {
			
			string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, 12);

			
			key = Encoding.ASCII.GetBytes(hashedPassword);
			
			
            
            iv = new byte[16] { 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 };
        }
		public string EncryptString(string plainText)
		{
		    
		    Aes encryptor = Aes.Create();

		    encryptor.Mode = CipherMode.CBC;
		    
		    
		    

		    
		    encryptor.Key = key.Take(32).ToArray();
		    encryptor.IV = iv;

		    
		    MemoryStream memoryStream = new MemoryStream();

		    
		    ICryptoTransform aesEncryptor = encryptor.CreateEncryptor();

		    
		    
		    CryptoStream cryptoStream = new CryptoStream(memoryStream, aesEncryptor, CryptoStreamMode.Write);

		    
		    byte[] plainBytes = Encoding.ASCII.GetBytes(plainText);

		    
		    cryptoStream.Write(plainBytes, 0, plainBytes.Length);

		    
		    cryptoStream.FlushFinalBlock();

		    
		    byte[] cipherBytes = memoryStream.ToArray();

		    
		    memoryStream.Close();
		    cryptoStream.Close();

		    
		    string cipherText = Convert.ToBase64String(cipherBytes, 0, cipherBytes.Length);

		    
		    return cipherText;
		}

		public string DecryptString(string cipherText)
		{
		    
		    Aes encryptor = Aes.Create();

		    encryptor.Mode = CipherMode.CBC;
		    
		    
		    

		    
		    encryptor.Key = key.Take(32).ToArray();
		    encryptor.IV = iv;

		    
		    MemoryStream memoryStream = new MemoryStream();

		    
		    ICryptoTransform aesDecryptor = encryptor.CreateDecryptor();

		    
		    
		    CryptoStream cryptoStream = new CryptoStream(memoryStream, aesDecryptor, CryptoStreamMode.Write);

		    
		    string plainText = String.Empty;

		    try
		    {
		        
		        byte[] cipherBytes = Convert.FromBase64String(cipherText);

		        
		        cryptoStream.Write(cipherBytes, 0, cipherBytes.Length);

		        
		        cryptoStream.FlushFinalBlock();

		        
		        byte[] plainBytes = memoryStream.ToArray();

		        
		        plainText = Encoding.ASCII.GetString(plainBytes, 0, plainBytes.Length);
		    }
		    finally
		    {
		        
		        memoryStream.Close();
		        cryptoStream.Close();
		    }

		    
		    return plainText;
		}
    }
}
