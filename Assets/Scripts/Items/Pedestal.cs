using Mirror;
using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class Pedestal : NetworkBehaviour
{
    [SerializeField]
    [NotNull(IgnorePrefab = true)]
    private NetworkItem networkItem = null;

    private Vector2 itemInitialPosition;

    private PedestalManager pedestalManager;

    public Vector2 ItemInitialPosition { get => itemInitialPosition; private set => itemInitialPosition = value; }
    public bool Stolen
    {
        get => stolen;
        private set
        {
            if(stolen == value)
            {
                return;
            }
            stolen = value;
            StolenStateChanged?.Invoke(this);
        }
    }

    public ItemScriptableObject Item { get; private set; }
    public Action<Pedestal> StolenStateChanged;
    private bool stolen;

    public override void OnStartServer()
    {
        ItemInitialPosition = networkItem.transform.position;
        Item = networkItem.Item;
        StartCoroutine(nameof(Register));
    }

    [Server]
    public void SetNetworkItem(NetworkItem networkItem)
    {
        this.networkItem = networkItem;

        this.Stolen = Vector2.Distance(networkItem.transform.position, ItemInitialPosition) > 1;
    }

    [Server]
    public void OnItemDestroy()
    {
        this.Stolen = true;
    }

    IEnumerator Register()
    {
        yield return new WaitUntil(() => PedestalManager.Instance != null);
        pedestalManager = PedestalManager.Instance;
        pedestalManager.Register(this);
    }
}
