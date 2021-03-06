using System;
using UnityEngine;

namespace Vaultify.VaultTypes
{
    [Serializable]
    public class MaterialVault : VaultBase<Material>
    {
        public MaterialVault(Material value) : base(value) { }

        public static implicit operator MaterialVault(Material value)
        {
            return new MaterialVault(value);
        }

        public static implicit operator Material(MaterialVault vault)
        {
            return vault.Value;
        }
    }
}
