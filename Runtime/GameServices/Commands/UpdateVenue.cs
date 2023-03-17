using Toolset;

namespace GameServices
{
    public class UpdateVenue : IMediatorCommand
    {
        public readonly string Venue;
        public readonly VenueAction Action;

        public UpdateVenue(VenueAction action, string venue = "")
        {
            Venue = venue;
            Action = action;
        }
    }
}