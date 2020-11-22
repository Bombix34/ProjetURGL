using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WarningFeedback : MonoBehaviour
{
    private bool isInit = false;

    private float baseScale;

    private void Start()
    {
        StartCoroutine(WaitForEndInitAnim());
    }

    private IEnumerator WaitForEndInitAnim()
    {
        yield return new WaitForSeconds(0.75f);
        baseScale = transform.localScale.x;
        isInit = true;
    }

    private void Update()
    {
        if (!isInit)
            return;
        transform.localScale = (Vector3.one * baseScale) + (Mathf.PingPong(Time.time * 2f, 0.5f) * Vector3.one);
    }

    public void DestroyFeedback()
    {
        transform.DOScale(Vector3.zero, 0.3f)
            .OnComplete(() => Destroy(this.gameObject));
    }
}
