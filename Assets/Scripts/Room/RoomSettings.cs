using Mirror;
using System;
using UnityEngine;

public class RoomSettings : NetworkBehaviour
{
    [SyncVar]
    public string roomUniqueIdentifier;
    [SerializeField]
    [NotNull]
    [SyncVar]
    private GameSettings settings = null;
    public GameSettings Settings { get => settings; }
    public static RoomSettings Instance { get; private set; }

    public override void OnStartServer()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(Instance.gameObject);
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        base.OnStartServer();
        this.roomUniqueIdentifier = Guid.NewGuid().ToString();
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (Instance != null && Instance != this)
        {
            Destroy(Instance.gameObject);
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnDestroy()
    {
        if(Instance == this)
        {
            Instance = null;
        }
    }
}
