using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;

using Vaultify.VaultTypes;

namespace Vaultify
{
    public static class VaultifyCore
    {
        public static event Action ValueChangedDirectly;

        static VaultifyCore()
        {
            ValueChangedDirectly += () =>
                    Debug.unityLogger .LogException(
                        new VaultTypeHackedException("Value changed directly"));
        }

        public static void FireValueChangedDirectly() =>
            ValueChangedDirectly?.Invoke();

        public static string ToSHA256(this object obj) =>
            ComputeSHA256(BitConverter.GetBytes(obj.GetHashCode()));


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
    }
}
