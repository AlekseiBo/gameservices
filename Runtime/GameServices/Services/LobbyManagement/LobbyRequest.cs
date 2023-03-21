using System.Threading.Tasks;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

namespace GameServices
{
    internal class LobbyRequest
    {
        public async Task<Lobby> CreateLobby(CreateLobbyData data)
        {
            try
            {
                return await LobbyService.Instance.CreateLobbyAsync(data.Name, data.MaxPlayers, data.Options);
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e.Message);
                return null;
            }
        }

        public async Task<Lobby> JoinLobby(string lobbyId)
        {
            try
            {
                return await LobbyService.Instance.JoinLobbyByIdAsync(lobbyId);
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e.Message);
                return null;
            }
        }

        public async Task<Lobby> JoinOrReconnectLobby(Lobby lobby, string playerId)
        {
            try
            {
                var player = lobby.Players.Find(p => p.Id == playerId);
                return player == null
                    ? await LobbyService.Instance.JoinLobbyByIdAsync(lobby.Id)
                    : await LobbyService.Instance.ReconnectToLobbyAsync(lobby.Id);
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e.Message);
                return null;
            }
        }

        public async Task<Lobby> JoinLobbyByCode(string code)
        {
            try
            {
                return await LobbyService.Instance.JoinLobbyByCodeAsync(code);
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e.Message);
                return null;
            }
        }

        public async Task<Lobby> UpdateLobby(string lobbyId, UpdateLobbyOptions options)
        {
            try
            {
                return await LobbyService.Instance.UpdateLobbyAsync(lobbyId, options);
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e.Message);
                return null;
            }
        }

        public async Task<Lobby> UpdateLobbyPlayer(string lobbyId, string playerId, UpdatePlayerOptions options)
        {
            try
            {
                return await LobbyService.Instance.UpdatePlayerAsync(lobbyId, playerId, options);
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e.Message);
                return null;
            }
        }

        public async Task DeleteLobby(string lobbyId)
        {
            try
            {
                await LobbyService.Instance.DeleteLobbyAsync(lobbyId);
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e.Message);
            }
        }

        public async Task LeaveLobby(string lobbyId, string playerId)
        {
            try
            {
                await LobbyService.Instance.RemovePlayerAsync(lobbyId, playerId);
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e.Message);
            }
        }
    }
}