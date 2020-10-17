using System;

namespace Vaultify.VaultTypes
{
    [Serializable]
    public class IntVault : VaultBase<int>
    {
        public IntVault(int value) : base(value) { }

        public static implicit operator IntVault(int value) =>
            new IntVault(value);

        public static implicit operator int(IntVault vault) =>
            vault.Value;
    }
}
