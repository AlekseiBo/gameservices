using Toolset;
using UnityEngine.Events;

namespace GameServices
{
    public class ShowMessage : IMediatorCommand
    {
        public readonly string Title;
        public readonly string Message;
        public readonly UnityAction Action;

        public ShowMessage(string title, string message, UnityAction action = null)
        {
            Title = title;
            Message = message;
            Action = action;
        }
    }
}