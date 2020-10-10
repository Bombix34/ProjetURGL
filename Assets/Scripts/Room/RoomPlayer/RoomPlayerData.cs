using Mirror;
using UnityEngine;

public class RoomPlayerData : NetworkBehaviour
{
    [SyncVar]
    private string playerIdentifier;

    public string GetPlayerIndentifier()
    {
        if (string.IsNullOrWhiteSpace(playerIdentifier))
        {
            this.playerIdentifier = $"Tombeur {Random.Range(1000, 9999)}";
        }

        return this.playerIdentifier;
    }
}
