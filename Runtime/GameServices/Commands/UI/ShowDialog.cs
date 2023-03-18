using Toolset;
using UnityEngine.Events;

namespace GameServices
{
    public class ShowDialog : IMediatorCommand
    {
        public readonly string Title;
        public readonly string Message;
        public readonly string FirstTitle;
        public readonly string SecondTitle;
        public readonly UnityAction FirstAction;
        public readonly UnityAction SecondAction;

        public ShowDialog(
            string title,
            string message,
            string firstTitle = "",
            UnityAction firstAction = null,
            string secondTitle = "",
            UnityAction secondAction = null)
        {
            Title = title;
            Message = message;
            FirstTitle = firstTitle;
            FirstAction = firstAction;
            SecondTitle = secondTitle;
            SecondAction = secondAction;
        }
    }
}