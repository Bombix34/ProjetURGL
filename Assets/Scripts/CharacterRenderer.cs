using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CharacterRenderer : NetworkBehaviour
{
    [SyncVar]
    private bool isRendererFlip = false;

    public bool IsRendererFlip
    {
        get => isRendererFlip;
        set
        {
            isRendererFlip = value;
            if (isRendererFlip)
                transform.localScale = new Vector3(-1f, 1f, 1f);
            else
                transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

}
