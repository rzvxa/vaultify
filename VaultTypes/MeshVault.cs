using System;
using UnityEngine;

namespace Vaultify.VaultTypes
{
    [Serializable]
    public class MeshVault : VaultBase<Mesh>
    {
        public MeshVault(Mesh value) : base(value) { }

        public static implicit operator MeshVault(Mesh value)
        {
            return new MeshVault(value);
        }

        public static implicit operator Mesh(MeshVault vault)
        {
            return vault.Value;
        }
    }
}
