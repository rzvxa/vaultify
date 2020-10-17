using System;
using UnityEngine;
using UnityEngine.Serialization;

using Vaultify;

namespace Vaultify.VaultTypes
{
    [Serializable]
    public abstract class VaultBase { }

    [Serializable]
    public abstract class VaultBase<T> : VaultBase, ISerializationCallbackReceiver
    {
        [SerializeField] private T _value;

        private string _hash;

        public T Value
        {
            get
            {
                IsValid();
                return _value;
            }
            set
            {
                _value = value;
                CalculateVaultValue();
            }
        }

        public VaultBase(T value) => Value = value;

        public bool IsValid()
        {
            if (_value.ToSHA256() == _hash)
                return true;
            VaultifyCore.FireValueChangedDirectly();
            return false;
        }

        public void OnBeforeSerialize() { }

        public void OnAfterDeserialize() => CalculateVaultValue();


        private void CalculateVaultValue() => _hash = _value.ToSHA256();
    }
}
