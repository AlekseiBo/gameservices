using System.Collections;
using System.Linq;
using Toolset;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

namespace GameServices
{
    internal class LobbyRoutine
    {
        private const float HEARTBEAT_TIMEOUT = 10f;
        private const float UPDATE_PLAYERS_TIMEOUT = 5f;

        private Coroutine heartbeatCoroutine;
        private Coroutine lobbyActivityCoroutine;
        private Coroutine updatePlayersCoroutine;

        private bool heartbeatInProgress;
        private bool lobbyActivityInProgress;
        private bool updatePlayersInProgress;

        private Lobby currentLobby;
        private string playersList;

        public void Start(Lobby lobby)
        {
            currentLobby = lobby;
            playersList = "";
            heartbeatCoroutine = CoroutineRunner.Start(RunHeartbeat());
            //lobbyActivityCoroutine = CoroutineRunner.Start(RunActivityCheck());
        }

        public void Stop()
        {
            if (heartbeatCoroutine != null) CoroutineRunner.Stop(heartbeatCoroutine);
            if (lobbyActivityCoroutine != null) CoroutineRunner.Stop(lobbyActivityCoroutine);
            currentLobby = null;
        }

        private IEnumerator RunHeartbeat()
        {
            while (currentLobby != null)
            {
                yield return Utilities.WaitFor(HEARTBEAT_TIMEOUT);
                SendHeartbeat();
                while (heartbeatInProgress) yield return null;
            }
        }

        private IEnumerator RunActivityCheck()
        {
            if (GameData.Get<NetState>(Key.PlayerNetState) != NetState.Dedicated) yield break;

            while (currentLobby != null)
            {
                yield return Utilities.WaitFor(GameData.Get<float>(Key.ServerActivityTimer) * 60f);
                CheckLobbyActivity();
                while (lobbyActivityInProgress) yield return null;
            }
        }

        private async void SendHeartbeat()
        {
            heartbeatInProgress = true;

            try
            {
                await LobbyService.Instance.SendHeartbeatPingAsync(currentLobby.Id);
            }
            catch (LobbyServiceException e)
            {
                Debug.Log($"Heartbeat Routine: {e.Message}");
                RestartLobby();
            }

            heartbeatInProgress = false;
        }

        private async void CheckLobbyActivity()
        {
            Debug.Log($"Lobby Activity Routine: Checking...");
            lobbyActivityInProgress = true;

            try
            {
                currentLobby = await LobbyService.Instance.GetLobbyAsync(currentLobby.Id);
                if (currentLobby.Players.Count <= 1) RestartLobby();
            }
            catch (LobbyServiceException e)
            {
                Debug.Log($"Lobby Activity Routine: {e.Message}");
                RestartLobby();
            }

            lobbyActivityInProgress = false;
        }

        private static void RestartLobby() =>
            Command.Publish(new UpdateVenue(VenueAction.Exit, GameData.Get<string>(Key.CurrentVenue)));
    }
}