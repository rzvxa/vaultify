using System;

namespace Vaultify.VaultTypes
{
    [Serializable]
    public class StringVault : VaultBase<string>
    {
        public StringVault(string value) : base(value) { }

        public static implicit operator StringVault(string value)
        {
            return new StringVault(value);
        }

        public static implicit operator string(StringVault vault)
        {
            return vault.Value;
        }
    }
}
