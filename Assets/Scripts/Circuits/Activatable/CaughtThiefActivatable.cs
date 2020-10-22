
public class CaughtThiefActivatable : BaseActivatable
{
    internal override void OnActivate()
    {
        GetComponent<IPlayerManager>().GetCaught();
    }

    internal override void OnDeactivate()
    {
        throw new System.NotImplementedException();
    }
}
