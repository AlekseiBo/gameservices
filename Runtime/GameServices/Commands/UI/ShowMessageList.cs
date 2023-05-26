using Toolset;

namespace GameServices
{
    public class ShowMessageList : IMediatorCommand
    {
        public string PlayerId;

        public ShowMessageList(string playerId = "")
        {
            PlayerId = playerId;
        }
    }
}