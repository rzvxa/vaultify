using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Linq;

using Vaultify.VaultTypes;

namespace Vaultify
{
    public static class VaultifyCore
    {
        public static event Action ValueChangedDirectly;

        private static bool _isInitialized;
        private static string _secret;
        private static int _rfc2898KeygenIterations;
        private static int _rijndaelKeySizeInBits;

        public static void Initialize(string secret,
                                      int rfc2898KeygenIterations = 1000,
                                      int rijndaelKeySizeInBits = 256)
        {
            if (_isInitialized)
                throw new Exception("Vaultify is already initialized");
            _secret = secret;
            _rfc2898KeygenIterations = rfc2898KeygenIterations;
            _rijndaelKeySizeInBits = rijndaelKeySizeInBits;
            _isInitialized = true;

        }

        public static void FireValueChangedDirectly()
        {
            ValueChangedDirectly?.Invoke();
        }

        public static string ToSHA256(this object obj)
        {
            return ComputeSHA256(BitConverter.GetBytes(obj.GetHashCode()));
        }


        public static string ComputeSHA256(byte[] bytes)
        {
            var sha2 = new SHA256CryptoServiceProvider();
            try
            {
                var result = sha2.ComputeHash(bytes);
                return BitConverter.ToString(result);
            }
            catch (ArgumentNullException e)
            {
                Debug.unityLogger.LogException(e);
                return null;
            }
        }

        public static string Encrypt(string plainText)
        {
            // Salt and IV is randomly generated each time, but is preprended to encrypted cipher text
            // so that the same Salt and IV values can be used when decrypting.
            var saltStringBytes = Generate256BitsOfRandomEntropy();
            var ivStringBytes = Generate256BitsOfRandomEntropy();
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            using (var password = new Rfc2898DeriveBytes(_secret, saltStringBytes, _rfc2898KeygenIterations))
            {
                var keyBytes = password.GetBytes(_rijndaelKeySizeInBits / 8);
                using (var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.BlockSize = 256;
                    symmetricKey.Mode = CipherMode.CBC;
                    symmetricKey.Padding = PaddingMode.PKCS7;
                    using (var encryptor = symmetricKey.CreateEncryptor(keyBytes, ivStringBytes))
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                            {
                                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                                cryptoStream.FlushFinalBlock();
                                // Create the final bytes as a concatenation of the random salt bytes, the random iv bytes and the cipher bytes.
                                var cipherTextBytes = saltStringBytes;
                                cipherTextBytes = cipherTextBytes.Concat(ivStringBytes).ToArray();
                                cipherTextBytes = cipherTextBytes.Concat(memoryStream.ToArray()).ToArray();
                                memoryStream.Close();
                                cryptoStream.Close();
                                return Convert.ToBase64String(cipherTextBytes);
                            }
                        }
                    }
                }
            }
        }

        public static string Decrypt(string cipherText)
        {
            // Get the complete stream of bytes that represent:
            // [32 bytes of Salt] + [32 bytes of IV] + [n bytes of CipherText]
            var cipherTextBytesWithSaltAndIv = Convert.FromBase64String(cipherText);
            // Get the saltbytes by extracting the first 32 bytes from the supplied cipherText bytes.
            var saltStringBytes = cipherTextBytesWithSaltAndIv.Take(_rijndaelKeySizeInBits / 8).ToArray();
            // Get the IV bytes by extracting the next 32 bytes from the supplied cipherText bytes.
            var ivStringBytes = cipherTextBytesWithSaltAndIv.Skip(_rijndaelKeySizeInBits / 8).Take(_rijndaelKeySizeInBits / 8).ToArray();
            // Get the actual cipher text bytes by removing the first 64 bytes from the cipherText string.
            var cipherTextBytes = cipherTextBytesWithSaltAndIv.Skip((_rijndaelKeySizeInBits / 8) * 2).Take(cipherTextBytesWithSaltAndIv.Length - ((_rijndaelKeySizeInBits / 8) * 2)).ToArray();

            using (var password = new Rfc2898DeriveBytes(_secret, saltStringBytes, _rfc2898KeygenIterations))
            {
                var keyBytes = password.GetBytes(_rijndaelKeySizeInBits / 8);
                using (var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.BlockSize = 256;
                    symmetricKey.Mode = CipherMode.CBC;
                    symmetricKey.Padding = PaddingMode.PKCS7;
                    using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, ivStringBytes))
                    {
                        using (var memoryStream = new MemoryStream(cipherTextBytes))
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                            {
                                var plainTextBytes = new byte[cipherTextBytes.Length];
                                var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                                memoryStream.Close();
                                cryptoStream.Close();
                                return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                            }
                        }
                    }
                }
            }
        }

        private static byte[] Generate256BitsOfRandomEntropy()
        {
            var randomBytes = new byte[32]; // 32 Bytes will give us 256 bits.
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                // Fill the array with cryptographically secure random bytes.
                rngCsp.GetBytes(randomBytes);
            }
            return randomBytes;
        }
    }
}
