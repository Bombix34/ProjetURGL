using Mirror;
using System;
public class RoomPlayerData : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnPlayerIdentifierChange))]
    private string playerIdentifier;
    [SyncVar(hook = nameof(OnPlayerTypeChange))]
    private PlayerType playerType;

    public string PlayerIndentifier => this.playerIdentifier;

    public PlayerType PlayerType { get => playerType; }
    public static RoomPlayerData LocalPlayer { get; private set; }
    public Action<PlayerType> onPlayerTypeChange;
    public Action<string> onPlayerIdentifierChange;

    public override void OnStartServer()
    {
        base.OnStartServer();
        this.playerType = PlayerType.THIEF;
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        LocalPlayer = this;
        this.CmdChangePseudo(AccountManager.Instance.Account.Pseudo);
    }

    [Command]
    public void CmdChangePseudo(string pseudo)
    {
        this.playerIdentifier = pseudo;
    }

    private void OnPlayerIdentifierChange(string oldPseudo, string newPseudo)
    {
        this.onPlayerIdentifierChange?.Invoke(newPseudo);
    }

    [Command]
    public void CmdChangePlayerType(PlayerType playerType)
    {
        this.playerType = playerType;
    }

    private void OnPlayerTypeChange(PlayerType oldPlayerType, PlayerType newPlayerType)
    {
        this.onPlayerTypeChange?.Invoke(newPlayerType);
    }
}
