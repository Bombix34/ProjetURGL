using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WarningFeedback : MonoBehaviour
{
    private bool isInit = false;

    private void Start()
    {
        StartCoroutine(WaitForEndInitAnim());
    }

    private IEnumerator WaitForEndInitAnim()
    {
        yield return new WaitForSeconds(0.75f);
        isInit = true;
    }

    private void Update()
    {
        if (!isInit)
            return;
        transform.localScale = (Vector3.one * 3f) + (Mathf.PingPong(Time.time * 2f, 1.5f) * Vector3.one);
    }

    public void DestroyFeedback()
    {
        transform.DOScale(Vector3.zero, 0.3f)
            .OnComplete(() => Destroy(this.gameObject));
    }
}
