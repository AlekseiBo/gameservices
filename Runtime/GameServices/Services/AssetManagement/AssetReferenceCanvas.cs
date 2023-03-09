using System;
using GameServices;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

[Serializable]
public class AssetReferenceCanvas : AssetReference
{
    public AssetReferenceCanvas(string guid) : base(guid)
    {
    }

    public override bool ValidateAsset(string path) => path.EndsWith("Canvas.prefab");
}