using System.Linq;
using UnityEngine;
using Toolset;

namespace GameServices.CodeBlocks
{
    [CreateAssetMenu(fileName = "ReadCommandLine", menuName = "Code Blocks/Initialization/Read Command Line",
        order = 0)]
    public class ReadCommandLine : CodeBlock
    {
        private const string RELAY_SERVER = "relay-server";
        private const string FRAME_RATE = "fps";
        private const string MAX_PLAYERS = "max-players";
        private const string VENUE_ADDRESS = "address";
        private const string SERVER_ACTIVITY = "activity-timer";

        protected override void Execute()
        {
            SetGameDataEntries();
            SetTargetFramesPerSecond();
            Complete(true);
        }

        private void SetGameDataEntries()
        {
#if UNITY_EDITOR
            return;
#endif
            if (CommandLineArgument(RELAY_SERVER))
                GameData.Set(Key.PlayerNetState, NetState.Dedicated);

            var address = CommandLineValue(VENUE_ADDRESS);
            if (!string.IsNullOrEmpty(address))
                GameData.Set(Key.RequestedVenue, address);

            var maxPlayers = CommandLineValue(MAX_PLAYERS);
            if (!string.IsNullOrEmpty(maxPlayers))
                GameData.Set(Key.LobbyMaxPlayers, int.Parse(maxPlayers));

            var activityTimer = CommandLineValue(SERVER_ACTIVITY);
            if (!string.IsNullOrEmpty(activityTimer))
                GameData.Set(Key.ServerActivityTimer, float.Parse(activityTimer));
        }

        private void SetTargetFramesPerSecond()
        {
            var fps = CommandLineValue(FRAME_RATE);
            if (!string.IsNullOrEmpty(fps)) Application.targetFrameRate = int.Parse(fps);
        }

        private bool CommandLineArgument(string argument)
        {
            var args = System.Environment.GetCommandLineArgs();
            return args.Any(t => t.Contains($"-{argument}"));
        }

        private string CommandLineValue(string argument)
        {
            var args = System.Environment.GetCommandLineArgs();

            for (var i = 0; i < args.Length; i++)
                if (args[i].Contains($"-{argument}"))
                    return args[i + 1];

            return "";
        }
    }
}