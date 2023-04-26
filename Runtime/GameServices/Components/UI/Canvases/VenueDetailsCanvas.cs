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

        private VenueStaticData venueData;

        public override void UpdateCanvas(IMediatorCommand command)
        {
            var data = command as ShowVenueDetails;
            venueData = data.VenueData;
            venueName.text = venueData.Name;
            playerCounter.text = $"Online: {data.PlayerCounter}";
            ApplyIcon(venueData.Icon);
        }

        private void ApplyIcon(Sprite sprite)
        {
            iconImage.sprite = sprite;
            iconImage.GetComponent<AspectRatioFitter>().aspectRatio =
                sprite.texture.width / (float)sprite.texture.height;
        }
    }
}