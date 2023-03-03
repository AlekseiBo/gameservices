using Unity.Netcode.Components;

namespace Scripts
{
        public class ClientNetworkAnimator : NetworkAnimator
        {
                protected override bool OnIsServerAuthoritative()
                {
                        return false;
                }
        }
}