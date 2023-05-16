using Unity.Netcode.Components;

namespace GameServices
{
        public class ClientNetworkAnimator : NetworkAnimator
        {
                protected override bool OnIsServerAuthoritative()
                {
                        return false;
                }
        }
}