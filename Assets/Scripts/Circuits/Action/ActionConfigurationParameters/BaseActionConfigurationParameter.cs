using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseActionConfigurationParameter
{
    [SerializeField]
    private bool enabled = false;

    public bool Enabled { get => enabled; }

    public virtual void Init()
    {

    }

    public bool CanDoAction()
    {
        if (!enabled)
        {
            return true;
        }
        return this.CanDoActionIfEnabled();
    }
    protected abstract bool CanDoActionIfEnabled();

    public void OnAction()
    {
        if (!enabled)
        {
            return;
        }
        this.OnActionIfEnabled();
    }
    protected abstract void OnActionIfEnabled();
}
