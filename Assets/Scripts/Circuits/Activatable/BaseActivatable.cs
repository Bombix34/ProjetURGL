using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseActivatable : MonoBehaviour
{
    public virtual void Activate()
    {
        this.OnActivate();
    }
    public virtual void Deactivate()
    {
        this.OnDeactivate();
    }

    public abstract void OnActivate();
    public abstract void OnDeactivate();
}
