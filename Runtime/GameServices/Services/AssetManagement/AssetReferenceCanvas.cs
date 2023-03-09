using System;
using UnityEngine.AddressableAssets;

[Serializable]
public class AssetReferenceCanvas : AssetReference
{
    public AssetReferenceCanvas(string guid) : base(guid)
    {
    }

    public override bool ValidateAsset(string path) => path.EndsWith("Canvas.prefab");
}