using Mirror;
using UnityEngine;

public class ValuableItemTrigger : NetworkBehaviour
{
    [ServerCallback]
    void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag(Tags.THIEF_TAG))
        {
            return;
        }
        if (col.GetComponent<Inventory>().HasValuableItem)
        {
            GameManager.Instance.EndGame(VictoryType.THIEVES_VICTORY);
        }
    }
}
