using System;
using UnityEngine;

namespace Vaultify.VaultTypes
{
    [Serializable]
    public class SpriteVault : VaultBase<Sprite>
    {
        public SpriteVault(Sprite value) : base(value) { }

        public static implicit operator SpriteVault(Sprite value) =>
            new SpriteVault(value);

        public static implicit operator Sprite(SpriteVault vault) =>
            vault.Value;
    }
}
