using Toolset;
using UnityEngine.Events;

namespace GameServices
{
    public class ShowMessage : IMediatorCommand
    {
        public readonly string Title;
        public readonly string Message;
        public readonly UnityAction Call;

        public ShowMessage(string title, string message, UnityAction action = null)
        {
            Title = title;
            Message = message;
            Call = action;
        }
    }
}