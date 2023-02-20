using System;
using Toolset;
using UnityEngine;

namespace GameServices.CodeBlocks
{
    [CreateAssetMenu(fileName = "LoadVenueRequested", menuName = "Code Blocks/Load Venue Requested", order = 0)]
    public class LoadVenueRequested : CodeBlock
    {
        protected override void Execute()
        {
            GameData<Key>.Subscribe<bool>(Key.LoadVenueRequested, OnLoadVenueRequested);
        }

        private void OnLoadVenueRequested(DataEntry<bool> entry)
        {
            GameData<Key>.RemoveSubscriber<bool>(Key.LoadVenueRequested, OnLoadVenueRequested);
            Complete(true);
        }
    }
}