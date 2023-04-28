using Toolset;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace GameServices
{
    public class VenueDetailsCanvas : BaseCanvas
    {
        [Space]
        [SerializeField] private Image iconImage;
        [SerializeField] private TextMeshProUGUI venueName;
        [SerializeField] private TextMeshProUGUI playerCounter;
        [SerializeField] private TextMeshProUGUI description;

        private VenueStaticData venueData;

        public override void UpdateCanvas(IMediatorCommand command)
        {
            var data = command as ShowVenueDetails;
            venueData = data.VenueData;
            venueName.text = venueData.Name;
            playerCounter.text = $"Online: {data.PlayerCounter}";
            description.text = venueData.Description;
            ApplyIcon(venueData.Icon);
        }

        public void SelectVenue()
        {
            GameData.Set(Key.RequestedVenue, venueData.Address);
            Command.Publish(new SelectAvatarProfile(venueData.AvatarGroup));
        }

        private void ApplyIcon(Sprite sprite)
        {
            iconImage.sprite = sprite;
            iconImage.GetComponent<AspectRatioFitter>().aspectRatio =
                sprite.texture.width / (float)sprite.texture.height;
        }
    }
}