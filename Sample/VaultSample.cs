using UnityEngine;
using Vaultify.VaultTypes;

namespace Vaultify.Sample
{
    public class VaultSample : MonoBehaviour
    {
        public IntVault MyIntiger = 999;
        public SpriteVault MySprite;
        [Header("Private with SerializeField attribute")]
        [SerializeField] private FloatVault _myFloat = 323f;
        [SerializeField] private ColorVault _myColor;
        [SerializeField] private MeshVault _myMesh;
        [SerializeField] private MaterialVault _myMaterial;
        [SerializeField] private StringVault _myString = "Default string is here";
    }

}
