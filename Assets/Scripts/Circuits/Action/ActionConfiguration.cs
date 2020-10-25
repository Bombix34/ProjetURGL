using System;
using UnityEngine;

[Serializable]
public class ActionConfiguration
{
    [SerializeField]
    private ActionTypes actionType;
    [SerializeField]
    private float cooldown;
    private WaitCoroutine _waitCoroutine;

    public ActionTypes ActionType => actionType;
    public float Cooldown => cooldown;

    public WaitCoroutine WaitCoroutine
    {
        get
        {
            if(_waitCoroutine == null)
            {
                _waitCoroutine = new WaitCoroutine(this.cooldown);
            }
            return _waitCoroutine;
        }
    }

    public bool CanDoAction()
    {
        return this._waitCoroutine.IsWaiting == false;
    }
}
