namespace GameServices
{
    public enum LobbyRequestType
    {
        Query,
        Create,
        Join,
        QuickJoin,
        GetLobby,
        DeleteLobby,
        UpdateLobby,
        UpdatePlayer,
        LeaveLobbyOrRemovePlayer,
        HeartBeat
    }
}