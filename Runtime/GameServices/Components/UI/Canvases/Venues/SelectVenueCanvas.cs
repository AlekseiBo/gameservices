using UnityEngine;

namespace GameServices
{
    public class SelectVenueCanvas : BaseCanvas
    {
        [SerializeField] private VenueSelectionList venueList;

        [ContextMenu("Show")]
        public override void ShowCanvas()
        {
            venueList.BuildList();
            base.ShowCanvas();
        }
    }
}