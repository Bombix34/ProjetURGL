using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSwitch : MonoBehaviour
{

    [SerializeField]
    private Tags.TagSelection TagSelection = Tags.TagSelection.THIEF;
    [SerializeField]
    private BaseActivatable activatable;

    public BaseActivatable Activatable { get => activatable; set => activatable = value; }
    public virtual void OnActivate()
    {
        Activatable.CmdActivate();
    }
    public virtual void OnDeactivate()
    {
        Activatable.CmdDeactivate();
    }

    private void OnDrawGizmos()
    {
        if (this.Activatable == null)
        {
            return;
        }
        Gizmos.color = Color.red;
        Gizmos.DrawLine(this.transform.position, this.Activatable.transform.position);
    }

    protected internal bool IsTagValid(Collider2D collider2D)
    {
        return collider2D.IsTagValid(TagSelection);
    }
}
