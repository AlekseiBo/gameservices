﻿using System;
using Toolset;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameServices.CodeBlocks
{
    [CreateAssetMenu(fileName = "Register Canvas Manager",
        menuName = "Code Blocks/Initialization/Register Canvas Manager", order = 0)]
    public class RegisterCanvasManager : CodeBlock
    {
        [SerializeField] private AssetReferenceGameObject prefab;

        protected override async void Execute()
        {
            if (Services.All.Single<ICanvasManager>() == null)
            {
                Services.All.RegisterSingle<ICanvasManager>(new CanvasManager());
                var canvasContainer =
                    await Services.All.Single<IAssetProvider>().Instantiate(prefab.AssetGUID, true);
                canvasContainer.name = "Canvas Container";
            }


            Complete(true);
        }
    }
}