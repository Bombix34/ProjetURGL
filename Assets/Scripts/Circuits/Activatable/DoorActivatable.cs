using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorActivatable : BaseActivatable
{
    public override void OnActivate()
    {
        Debug.Log("open");
    }

    public override void OnDeactivate()
    {
        Debug.Log("close");
    }
}
