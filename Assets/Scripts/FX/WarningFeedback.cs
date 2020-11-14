using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WarningFeedback : MonoBehaviour
{
    private bool isInit = false;

    private void Start()
    {
        transform.localScale = Vector3.zero;
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(transform.DOScale(Vector3.one * 3f, 0.3f))
            .Append(transform.DOPunchScale(Vector3.one * 5f, 0.2f, 1, 1))
            .OnComplete(() => isInit = true);
        mySequence.Play();
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
