﻿using Mirror;

public class ThiefManager : PlayerManager, ICaughtable
{
    protected override void OnStart()
    {
    }

    [Server]
    public void GetCaught()
    {
        this.RpcDisable();
    }

    [ClientRpc]
    private void RpcDisable()
    {
        this.gameObject.SetActive(false);
        if (isLocalPlayer)
        {
            CameraManager.Instance.NextPlayer();
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
        inventory.DropItem(item);
    }
}