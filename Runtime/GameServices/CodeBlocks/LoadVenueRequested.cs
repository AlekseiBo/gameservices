using System;
using Framework;
using GameServices.GameDataService;
using UnityEngine;

namespace GameServices.CodeBlocks
{
    [CreateAssetMenu(fileName = "LoadVenueRequested", menuName = "Code Blocks/Load Venue Requested", order = 0)]
    public class LoadVenueRequested : CodeBlock
    {
        public override void Execute(CodeRunner runner, Action<bool> completed)
        {
            base.Execute(runner, completed);

            GameData.Subscribe<bool>(Key.LoadVenueRequested, OnLoadVenueRequested);

        }

        private void OnLoadVenueRequested(DataEntry<bool> entry)
        {
            GameData.RemoveSubscriber<bool>(Key.LoadVenueRequested, OnLoadVenueRequested);
            Completed?.Invoke(true);
        }
    }
}