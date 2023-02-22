using System.Linq;
using UnityEngine;
using Toolset;

namespace GameServices.CodeBlocks
{
    [CreateAssetMenu(fileName = "ReadCommandLine", menuName = "Code Blocks/Initialization/Read Command Line", order = 0)]
    public class ReadCommandLine : CodeBlock
    {
        private const string RELAY_SERVER = "relay-server";
        private const string FRAME_RATE = "fps";
        private const string MAX_PLAYERS = "max-players";
        private const string VENUE_ADDRESS = "address";

        protected override void Execute()
        {
            SetGameDataEntries();
            SetTargetFramesPerSecond();
            Complete(true);
        }

        private void SetGameDataEntries()
        {
            GameData.Set(Key.RelayServer, CommandLineArgument(RELAY_SERVER));

            var address = CommandLineValue(VENUE_ADDRESS);
            if (!string.IsNullOrEmpty(address)) GameData.Set(Key.ActiveVenue, address);

            var maxPlayers = CommandLineValue(MAX_PLAYERS);
            if (!string.IsNullOrEmpty(maxPlayers)) GameData.Set(Key.MaxPlayers, int.Parse(maxPlayers));
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