using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSwitch : MonoBehaviour
{

    [SerializeField]
    private TagSelection tagSelection = TagSelection.THIEF;
    [SerializeField]
    private BaseActivatable activatable = null;

    public BaseActivatable Activatable { get => activatable; }
    public TagSelection TagSelection { get => tagSelection; }

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
