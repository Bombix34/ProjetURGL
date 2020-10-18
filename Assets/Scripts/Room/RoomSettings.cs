using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSettings : NetworkBehaviour
{
    [SyncVar]
    public string roomUniqueIdentifier;
    public static RoomSettings Instance { get; private set; }

    public override void OnStartServer()
    {
        base.OnStartServer();
        this.roomUniqueIdentifier = Guid.NewGuid().ToString();
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        Instance = this;
    }
}
