using Mirror;
using System;
using UnityEngine;
public class RoomPlayerData : NetworkBehaviour
{
    [SyncVar]
    private string playerIdentifier;
    [SyncVar(hook = nameof(OnPlayerTypeChange))]
    private PlayerType playerType;

    public string PlayerIndentifier => this.playerIdentifier;

    public PlayerType PlayerType { get => playerType; }
    public static RoomPlayerData LocalPlayer { get; private set; }
    public Action<PlayerType> onPlayerTypeChange;

    public override void OnStartServer()
    {
        base.OnStartServer();
        this.playerIdentifier = $"Tombeur {UnityEngine.Random.Range(1000, 9999)}";
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

    private void OnPlayerTypeChange(PlayerType oldPlayerType, PlayerType newPlayerType)
    {
        this.onPlayerTypeChange(newPlayerType);
    }
}
