using Toolset;
using UnityEngine;

namespace GameServices
{
    public class LogMessage : IMediatorCommand
    {
        public readonly LogType LogType;
        public readonly string Message;

        public LogMessage(LogType logType, string message)
        {
            LogType = logType;
            Message = message;
        }
    }
}