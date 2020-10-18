# vaultify
A More secure way of storing data in Unity3D

It contains 2 parts, First is secure variables which will notify you when a player tries to change them in the memory directly.
Other part is a safe Save system working on top of PlayerPrefs class of Unity3D.

## How To Use
First clone or download this repository and add this to your project,
Next thing you should initialize VaultifyCore before using it, If you are not sure where to initialize it, just do it in awake event of a game object in your first scene.
```csharp
using Vaultify;
.
.
.
.
VaultifyCore.Initialize("put a very strong secret here");
```
Now you can use it everywhere you want.
### Vault Types
These types are more safe than usual because of a checksum system implemented in it.
Just use Vault types instead of normal types like this:
```csharp
using Vaultify.VaultTypes;
.
.
.
.
public IntVault MyIntiger = 1234;
public SpriteVault MySprite;
// SerializeField attribute is supported!
[SerializeField] private FloatVault _myFloat = 56.78f;
```
If you have a custom type or want to make an already existing type safe using Vaultify you have to create a new class extending `VaultBase<T>` by doing so you will get inspector and safety both at the same time, In Unity 2020 because of Generic Serialization you can skip this step and use `VaultBase<T>` directly but i still recommend you to use extending method because of implicit casts! Here is an example of creating vault for a hypothetical type called `MyCustomType`:
```csharp
using System;
using Vaultify.VaultTypes
.
.
.
.
[Serializable]
public class MyCustomTypeVault : VaultBase<MyCustomType>
{
  public MyCustomTypeVault(MyCustomType value) : base(value) { }
  public static implicit operator MyCustomTypeVault(MyCustomType value) =>
    new MyCustomTypeVault(value);

  public static implicit operator MyCustomType(MyCustomTypeVault vault) =>
    vault.Value;
}
```
### VaultPrefs
It's just an wrapper around PlayerPrefs which uses Rijndael Encryption to keep variable secure using a 256 byte key.
Using it is just like PlayerPrefs, Only diffrence is for HasKey and DeleteKey methods, you can't just use HasKey or DeleteKey, there is 3 HasKeys for diffrent types; There is HasKeyFloat, HasKeyInt and HasKeyString same is happening for DeleteKey, everything else is the same.
```csharp
using Vaultify;
.
.
.
.
VaultPrefs.SetFloat("myFloat", _myFloat);
VaultPrefs.SetInt("myInt", MyIntiger);
VaultPrefs.SetString("myString", _myString);

Debug.unityLogger.Log(VaultPrefs.GetFloat("myFloat"));
Debug.unityLogger.Log(VaultPrefs.GetInt("myInt"));
Debug.unityLogger.Log(VaultPrefs.GetString("myString"));
```
