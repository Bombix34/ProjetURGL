using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaSwitch : BaseSwitch
{
    private int playerInArea = 0;

    private void PlayerEnterArea(GameObject gameObject)
    {
        if (!this.PlayerRespectCondition(gameObject))
        {
            return;
        }
        if (playerInArea == 0)
        {
            this.OnActivate();
        }
        playerInArea++;
    }

    private void PlayerExitArea(GameObject gameObject)
    {
        if (!this.PlayerRespectCondition(gameObject))
        {
            return;
        }
        playerInArea--;

        if (playerInArea == 0)
        {
            this.OnDeactivate();
        }
    }

    internal virtual bool PlayerRespectCondition(GameObject gameObject)
    {
        return true;
    }

    [ServerCallback]
    void OnTriggerEnter2D(Collider2D col)
    {
        if (this.IsTagValid(col))
        {
            this.PlayerEnterArea(col.gameObject);
        }
    }

    [ServerCallback]
    void OnTriggerExit2D(Collider2D col)
    {
        if (this.IsTagValid(col))
        {
            this.PlayerExitArea(col.gameObject);
        }
    }
}
