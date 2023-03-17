using Toolset;
using UnityEngine;

namespace GameServices
{
    public class VenueActionRunner : MonoBehaviour
    {
        [SerializeField] private CodeRunner onExit;
        [SerializeField] private CodeRunner onChange;

        private void Awake() => Command.Subscribe<UpdateVenue>(Requested);

        private void Requested(UpdateVenue venueData)
        {
            GameData.Set(Key.RequestedVenue, venueData.Venue);

            switch (venueData.Action)
            {
                case VenueAction.Exit:
                    onExit.Run();
                    break;
                case VenueAction.Change:
                    onChange.Run();
                    break;
            }
        }

        private void OnDestroy() => Command.RemoveSubscriber<UpdateVenue>(Requested);
    }
}