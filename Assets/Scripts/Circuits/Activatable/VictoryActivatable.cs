using Mirror;
using UnityEngine;

public class VictoryActivatable : BaseActivatable
{
    [SerializeField]
    private VictoryType victoryType;
    internal override void OnActivate(NetworkConnectionToClient sender)
    {
        GameManager.Instance.EndGame(VictoryType.THIEVES_VICTORY);
    }

    internal override void OnDeactivate(NetworkConnectionToClient sender) { }
}
