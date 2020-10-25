using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitCoroutine
{
    private readonly float _seconds;
    public bool IsWaiting { get; private set; } = false;
    public WaitCoroutine(float seconds)
    {
        this._seconds = seconds;
    }

    public IEnumerator Wait()
    {
        this.IsWaiting = true;
        yield return new WaitForSeconds(this._seconds);
        this.IsWaiting = false;
    }
}
