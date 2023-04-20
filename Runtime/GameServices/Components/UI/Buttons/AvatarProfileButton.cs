using System.Collections;
using System.Collections.Generic;
using GameServices;
using TMPro;
using Toolset;
using UnityEngine;
using UnityEngine.UI;
using Avatar = GameServices.Avatar;

public class AvatarProfileButton : BaseButton
{
    [SerializeField] private RawImage previewImage;
    [SerializeField] private TextMeshProUGUI prefabName;

    private AvatarPersistentData data;

    public void Construct(AvatarPersistentData avatarData, Texture2D avatarIcon)
    {
        previewImage.texture = avatarIcon;
        prefabName.text = avatarData.Prefab;
        data = avatarData;
    }

    public void Construct(AvatarPersistentData avatarData)
    {
        data = avatarData;
    }

    protected override void OnClick()
    {
        UpdateAvatar();
    }

    public void EditAvatar()
    {
        UpdateAvatar();
        Command.Publish(new CustomizeAvatar());
    }

    public void RemoveAvatar()
    {
        Services.All.Single<IProgressProvider>().ProgressData.AvatarList.Remove(data.Prefab);
    }

    private void UpdateAvatar()
    {
        AvatarData.Set(Avatar.Prefab, data.Prefab);

        AvatarData.Set(Avatar.Hair, data.Hair);
        AvatarData.Set(Avatar.Top, data.Top);
        AvatarData.Set(Avatar.Bottom, data.Bottom);
        AvatarData.Set(Avatar.Shoes, data.Shoes);

        AvatarData.Set(Avatar.SkinColor, data.SkinColor);
        AvatarData.Set(Avatar.HairColor, data.HairColor);
        AvatarData.Set(Avatar.EyeColor, data.EyeColor);
        AvatarData.Set(Avatar.OutfitColor, data.OutfitColor);
    }
}