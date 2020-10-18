using Mirror;
using UnityEngine;

public enum PlayerType
{
    THIEF,
    VIGIL,
    SPECTATOR
}
public class RoomPlayerData : NetworkBehaviour
{
    [SyncVar]
    private string playerIdentifier;
    [SyncVar]
    private PlayerType playerType;

    public string PlayerIndentifier => this.playerIdentifier;

    public PlayerType PlayerType { get => playerType; }

    public override void OnStartServer()
    {
        base.OnStartServer();
        this.playerIdentifier = $"Tombeur {Random.Range(1000, 9999)}";
        this.playerType = PlayerType.THIEF;
    }

    [Command]
    public void CmdChangePlayerType(PlayerType playerType)
    {
        this.playerType = playerType;
    }
}
