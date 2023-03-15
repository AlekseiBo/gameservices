using System.Collections;
using System.Collections.Generic;
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
        private Lobby currentLobby;
        private string playersList;

        public void Start(Lobby lobby)
        {
            currentLobby = lobby;
            heartbeatCoroutine = CoroutineRunner.Start(RunHeartbeat());
            updatePlayersCoroutine = CoroutineRunner.Start(RunUpdatePlayers());
            lobbyActivityCoroutine = CoroutineRunner.Start(RunActivityCheck());
        }

        public void Stop()
        {
            if (heartbeatCoroutine != null) CoroutineRunner.Stop(heartbeatCoroutine);
            if (updatePlayersCoroutine != null) CoroutineRunner.Stop(updatePlayersCoroutine);
            if (lobbyActivityCoroutine != null) CoroutineRunner.Stop(lobbyActivityCoroutine);
            currentLobby = null;
        }

        private IEnumerator RunHeartbeat()
        {
            while (currentLobby != null)
            {
                yield return Utilities.WaitFor(HEARTBEAT_TIMEOUT);

                try
                {
                    LobbyService.Instance.SendHeartbeatPingAsync(currentLobby.Id);
                }
                catch (LobbyServiceException e)
                {
                    Debug.Log($"Heartbeat Routine: {e.Message}");
                    Command.Publish(new RestartLobby());
                }
            }
        }

        private IEnumerator RunUpdatePlayers()
        {
            while (currentLobby != null)
            {
                yield return Utilities.WaitFor(UPDATE_PLAYERS_TIMEOUT);

                UpdatePlayersList();
            }
        }

        private IEnumerator RunActivityCheck()
        {
            if (GameData.Get<NetState>(Key.PlayerNetState) != NetState.Dedicated) yield break;

            while (currentLobby != null)
            {
                yield return Utilities.WaitFor(GameData.Get<float>(Key.ServerActivityTimer) * 60f);

                CheckLobbyActivity();
            }
        }

        private async void UpdatePlayersList()
        {
            try
            {
                Debug.Log($"Update Players Routine: Checking...");
                currentLobby = await LobbyService.Instance.GetLobbyAsync(currentLobby.Id);

                var newList = currentLobby.Players.Aggregate("", (current, player) => current + $"{player.Id},");

                if (playersList == newList) return;

                playersList = newList;
                var options = new UpdateLobbyOptions
                {
                    Data = new Dictionary<string, DataObject>()
                    {
                        {
                            "Players", new DataObject(
                                visibility: DataObject.VisibilityOptions.Public,
                                value: playersList,
                                index: DataObject.IndexOptions.S1)
                        }
                    }
                };

                currentLobby = await LobbyService.Instance.UpdateLobbyAsync(currentLobby.Id, options);
            }
            catch (LobbyServiceException e)
            {
                Debug.Log($"Update Players Routine: {e.Message}");
            }
        }

        private async void CheckLobbyActivity()
        {
            Debug.Log($"Lobby Activity Routine: Checking...");
            try
            {
                currentLobby = await LobbyService.Instance.GetLobbyAsync(currentLobby.Id);
                if (currentLobby.Players.Count <= 1)
                    Command.Publish(new RestartLobby());
            }
            catch (LobbyServiceException e)
            {
                Debug.Log($"Lobby Activity Routine: {e.Message}");
                Command.Publish(new RestartLobby());
            }
        }
    }
}