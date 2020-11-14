using Mirror;
using System.Collections.Generic;
using System.Linq;

public class PedestalManager : NetworkBehaviour
{
    private readonly List<Pedestal> pedestals = new List<Pedestal>();
    [SyncVar(hook = nameof(OnHasAnyValuableItemsStolenChange))]
    private bool hasAnyValuableItemsStolen;
    public bool HasAnyValuableItemsStolen { get => hasAnyValuableItemsStolen; }
    public static PedestalManager Instance;

    public override void OnStartServer()
    {
        print("OnStartServer");
        Instance = this;
    }

    public void Register(Pedestal pedestal)
    {
        this.pedestals.Add(pedestal);
        pedestal.StolenStateChanged += this.RefreshPedestal;
    }

    private void RefreshPedestal()
    {
        this.hasAnyValuableItemsStolen = this.pedestals.Any(p => p.Item.IsValuableItem && p.Stolen);
    }

    private void OnHasAnyValuableItemsStolenChange(bool oldValue, bool newValue)
    {
        if (newValue)
        {
            print("ALARME_ON");
        }
        else
        {
            print("ALARME_OFF");
        }
    }
}
