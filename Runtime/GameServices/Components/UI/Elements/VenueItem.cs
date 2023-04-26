using TMPro;
using Toolset;
using UnityEngine;
using UnityEngine.UI;

namespace GameServices
{
    public class VenueItem : BaseButton
    {
        public string Address => venueData.Address;

        [SerializeField] private Image venueIcon;
        [SerializeField] private TextMeshProUGUI venueName;
        [SerializeField] private TextMeshProUGUI onlineCounter;

        private int playerCounter;
        private VenueStaticData venueData;

        public void Construct(VenueStaticData venue)
        {
            venueData = venue;
            venueName.text = venue.Name;

            ApplyIcon(venue.Icon);
        }

        public void UpdatePlayerCounter(int players)
        {
            playerCounter = players;
            onlineCounter.text = $"Online: {players}";
        }

        private void ApplyIcon(Sprite sprite)
        {
            venueIcon.sprite = sprite;
            venueIcon.GetComponent<AspectRatioFitter>().aspectRatio =
                sprite.texture.width / (float)sprite.texture.height;
        }

        protected override void OnClick()
        {
            Command.Publish(new ShowVenueDetails(venueData, playerCounter));
        }
    }
}