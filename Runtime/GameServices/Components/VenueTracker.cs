using Toolset;
using UnityEngine;

namespace GameServices
{
    public class VenueTracker : MonoBehaviour
    {
        [SerializeField] private CodeRunner onExit;
        [SerializeField] private CodeRunner onChange;

        private void Awake()
        {
            Command.Subscribe<UpdateVenue>(VenueRequest);
            Command.Subscribe<ActivatePlayerTrigger>(TriggerActivated);
        }

        private void OnDestroy() => RemoveSubscribers();

        private void OnApplicationQuit() => onExit.Run();

        private void TriggerActivated(ActivatePlayerTrigger trigger)
        {
            if (trigger.Player.IsOwnedByServer && trigger.Player.IsLocalPlayer)
            {
                var venue = trigger.Data as VenueStaticData;
                var venueAddress = venue.Address;
                var dialog = new ShowDialog(
                    "Teleport",
                    $"Do you want to go to {venue.Name}?",
                    "YES",
                    () => HostChangeVenue(venueAddress),
                    "NO"
                );

                Command.Publish(dialog);
            }

            if (!trigger.Player.IsOwnedByServer && trigger.Player.IsLocalPlayer)
            {
                var venue = trigger.Data as VenueStaticData;
                var venueAddress = venue.Address;
                var dialog = new ShowDialog(
                    "Teleport",
                    $"Do you want to go to {venue.Name}?",
                    "YES",
                    () => ClientChangeVenue(venueAddress),
                    "NO"
                );

                Command.Publish(dialog);
            }
        }

        private async void HostChangeVenue(string address)
        {
            await Services.All.Single<ILobbyProvider>().UpdateVenue(address);
            VenueRequest(new UpdateVenue(VenueAction.Change, address));
            FindObjectOfType<NetworkTracker>().ChangeVenueClientRpc(address);
        }

        private static void ClientChangeVenue(string venueAddress)
        {
            GameData.Set(Key.PlayerNetState, NetState.Client);
            Command.Publish(new UpdateVenue(VenueAction.Exit, venueAddress));
        }

        private void VenueRequest(UpdateVenue venueData)
        {
            GameData.Set(Key.RequestedVenue, venueData.Venue);

            switch (venueData.Action)
            {
                case VenueAction.Exit:
                    RemoveSubscribers();
                    onExit.Run();
                    break;
                case VenueAction.Change:
                    onChange.Run();
                    break;
            }
        }

        private void RemoveSubscribers()
        {
            Command.RemoveSubscriber<UpdateVenue>(VenueRequest);
            Command.RemoveSubscriber<ActivatePlayerTrigger>(TriggerActivated);
        }
    }
}