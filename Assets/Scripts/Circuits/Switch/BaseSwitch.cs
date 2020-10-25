using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class BaseSwitch : MonoBehaviour
{

    [SerializeField]
    private List<TagSelection> tagSelections = new List<TagSelection>();
    [SerializeField]
    private BaseActivatable activatable = null;

    public BaseActivatable Activatable { get => activatable; }
    public List<TagSelection> TagSelections { get => tagSelections; }

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
        return this.tagSelections.Any(tagSelection => collider2D.IsTagValid(tagSelection));
    }
}
