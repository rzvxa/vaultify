using System;
using UnityEngine;

namespace Vaultify.VaultTypes
{
    [Serializable]
    public class ColorVault : VaultBase<Color>
    {
        public ColorVault(Color value) : base(value) { }

        public static implicit operator ColorVault(Color value) =>
            new ColorVault(value);

        public static implicit operator Color(ColorVault vault) =>
            vault.Value;
    }
}
