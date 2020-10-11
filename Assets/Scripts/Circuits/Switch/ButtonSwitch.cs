using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSwitch : BaseSwitch
{
    [SerializeField]
    private float activatedDurationSecond;

    public float ActivatedDurationSecond { get => activatedDurationSecond; set => activatedDurationSecond = value; }

    private Coroutine waitForDeactivateCoroutine;

    public override void OnActivate()
    {
        base.OnActivate();
        if(waitForDeactivateCoroutine != null)
        {
            StopCoroutine(waitForDeactivateCoroutine);
        }
        waitForDeactivateCoroutine = StartCoroutine(WaitForDeactivate());
    }

    IEnumerator WaitForDeactivate()
    {
        yield return new WaitForSeconds(this.activatedDurationSecond);
        this.OnDeactivate();
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (!this.IsTagValid(col))
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            this.OnActivate();
        }
    }
}
