using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSwitch : MonoBehaviour
{
    [SerializeField]
    private BaseActivatable activatable;

    public BaseActivatable Activatable { get => activatable; set => activatable = value; }
    public virtual void OnActivate()
    {
        Activatable.Activate();
    }
    public virtual void OnDeactivate()
    {
        Activatable.Deactivate();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(this.transform.position, this.Activatable.transform.position);
    }
}
