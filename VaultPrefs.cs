using UnityEngine;

namespace Vaultify
{
    public static class VaultPrefs
    {
        private static readonly string KeyFormat = "{0}_{1}_vault"; // {type}_{key}_vault

        public static void DeleteAll()
        {
            PlayerPrefs.DeleteAll();
        }

        public static void DeleteFloat(string key)
        {
            key = string.Format(KeyFormat, "float", key);
            PlayerPrefs.DeleteKey(key);
        }

        public static void DeleteInt(string key)
        {
            key = string.Format(KeyFormat, "int", key);
            PlayerPrefs.DeleteKey(key);
        }

        public static void DeleteString(string key)
        {
            key = string.Format(KeyFormat, "string", key);
            PlayerPrefs.DeleteKey(key);
        }

        public static void Save() => PlayerPrefs.Save();

        public static bool HasKeyFloat(string key)
        {
            key = string.Format(KeyFormat, "float", key);
            return PlayerPrefs.HasKey(key);
        }

        public static bool HasKeyInt(string key)
        {
            key = string.Format(KeyFormat, "int", key);
            return PlayerPrefs.HasKey(key);
        }

        public static bool HasKeyString(string key)
        {
            key = string.Format(KeyFormat, "string", key);
            return PlayerPrefs.HasKey(key);
        }

        public static float GetFloat(string key, float @default = 0)
        {
            key = string.Format(KeyFormat, "float", key);
            return float.Parse(GetRaw(key, @default.ToString()));
        }

        public static int GetInt(string key, int @default = 0)
        {
            key = string.Format(KeyFormat, "int", key);
            return int.Parse(GetRaw(key, @default.ToString()));
        }

        public static string GetString(string key, string @default = "")
        {
            key = string.Format(KeyFormat, "string", key);
            return GetRaw(key, @default);
        }

        public static void SetFloat(string key, float value)
        {
            key = string.Format(KeyFormat, "float", key);
            SetRaw(key, value.ToString());
        }

        public static void SetInt(string key, int value)
        {
            key = string.Format(KeyFormat, "int", key);
            SetRaw(key, value.ToString());
        }

        public static void SetString(string key, string value)
        {
            key = string.Format(KeyFormat, "string", key);
            SetRaw(key, value);
        }

        private static void SetRaw(string key, string value)
        {
            PlayerPrefs.SetString(key, VaultifyCore.Encrypt(value));
        }

        private static string GetRaw(string key, string @default)
        {
            var value = PlayerPrefs.GetString(key, @default);
            if (value == @default)
                return value;
            return VaultifyCore.Decrypt(value);
        }
    }
}
