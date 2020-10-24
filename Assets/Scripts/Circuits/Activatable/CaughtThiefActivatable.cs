
public class CaughtThiefActivatable : BaseActivatable
{
    private void Start()
    {
        this.type = ActionTypes.ARREST_CHARACTER;
    }
    internal override void OnActivate()
    {
        var playerManager = GetComponent<PlayerManager>();
        if (playerManager != null)
        {
            GameManager.Instance.RemoveThief(gameObject);
        }

        GetComponent<IPlayerManager>().GetCaught();
    }

    internal override void OnDeactivate()
    {
        throw new System.NotImplementedException();
    }
}
