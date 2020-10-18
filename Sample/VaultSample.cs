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

        [ContextMenu("ClearPrefs")]
        private void ClearPrefs() => PlayerPrefs.DeleteAll();

        private void Awake()
        {
            VaultifyCore.Initialize("put a very strong secret here");
        }

        private void Start()
        {
            VaultPrefs.SetFloat("myFloat", _myFloat);
            VaultPrefs.SetInt("myInt", MyIntiger);
            VaultPrefs.SetString("myString", _myString);

            Debug.unityLogger.Log(VaultPrefs.GetFloat("myFloat"));
            Debug.unityLogger.Log(VaultPrefs.GetInt("myInt"));
            Debug.unityLogger.Log(VaultPrefs.GetString("myString"));
        }
    }

}
