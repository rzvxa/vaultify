using System;

namespace Vaultify.VaultTypes
{
    [Serializable]
    public class FloatVault : VaultBase<float>
    {
        public FloatVault(float value) : base(value) { }

        public static implicit operator FloatVault(float value) =>
            new FloatVault(value);

        public static implicit operator float(FloatVault vault) =>
            vault.Value;
    }
}
