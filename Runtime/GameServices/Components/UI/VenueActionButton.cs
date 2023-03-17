using Toolset;
using UnityEngine;

namespace GameServices
{
    public class VenueActionButton : BaseButton
    {
        [SerializeField] private VenueAction venueAction;
        [SerializeField] private string requestVenue;

        protected override void OnClick()
        {
            Command.Publish(new UpdateVenue(venueAction, requestVenue));
        }
    }
}