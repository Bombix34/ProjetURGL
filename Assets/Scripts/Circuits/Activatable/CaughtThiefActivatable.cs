using Mirror;

public class CaughtThiefActivatable : BaseActivatable
{
    public override ActionTypes ActionType => ActionTypes.ARREST_CHARACTER;

    internal override void OnActivate(NetworkConnectionToClient sender)
    {
        var playerManager = GetComponent<PlayerManager>();
        if (playerManager != null)
        {
            GameManager.Instance.RemoveThief(gameObject);
        }

        GetComponent<ICaughtable>().GetCaught();
    }

    internal override void OnDeactivate(NetworkConnectionToClient sender)
    {
        throw new System.NotImplementedException();
    }
}
