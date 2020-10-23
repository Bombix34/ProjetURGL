
public class CaughtThiefActivatable : BaseActivatable
{
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
