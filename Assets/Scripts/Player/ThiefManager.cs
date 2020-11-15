using Mirror;

public class ThiefManager : PlayerManager, ICaughtable
{
    protected override void OnStart()
    {
    }

    [Server]
    public void GetCaught()
    {
        this.inventory.DropAllItems(true);
        this.Alive = false;
        this.RpcDisable();
    }

    [ClientRpc]
    private void RpcDisable()
    {
        this.gameObject.SetActive(false);
        if (isLocalPlayer)
        {
            CameraManager.Instance.StartFollowPlayerCameraMovements(PlayerType.THIEF);
        }
    }

    protected override void OnPressAlternateActionButton()
    {
        this.CmdDropValuableItem();
    }

    [Command]
    private void CmdDropValuableItem()
    {
        var item = inventory.GetValuableItem();
        if (item is null)
        {
            return;
        }
        inventory.DropItem(item, false);
    }
}
