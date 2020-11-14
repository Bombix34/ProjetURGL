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
    public static RoomPlayerData LocalPlayer { get; private set; }

    public override void OnStartServer()
    {
        base.OnStartServer();
        this.playerIdentifier = $"Tombeur {Random.Range(1000, 9999)}";
        this.playerType = PlayerType.THIEF;
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        LocalPlayer = this;
    }

    [Command]
    public void CmdChangePlayerType(PlayerType playerType)
    {
        this.playerType = playerType;
    }
}
