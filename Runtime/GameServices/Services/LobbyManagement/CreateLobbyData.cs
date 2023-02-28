namespace GameServices
{
    public struct CreateLobbyData
    {
        public readonly string Name;
        public readonly int MaxPlayers;
        public readonly bool IsPrivate;

        public CreateLobbyData(string name, int maxPlayers, bool isPrivate)
        {
            Name = name;
            MaxPlayers = maxPlayers;
            IsPrivate = isPrivate;
        }
    }
}