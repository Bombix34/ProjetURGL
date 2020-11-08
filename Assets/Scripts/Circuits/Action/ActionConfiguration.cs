using System;
using UnityEngine;

[Serializable]
public class ActionConfiguration
{
    [SerializeField]
    private ActionTypes actionType = ActionTypes.NOT_SET;
    [SerializeField]
    private float cooldown = 0;

    public ActionTypes ActionType => actionType;
    public float Cooldown => cooldown;
    public bool Waiting
    {
        get
        {
            return TimeUntilNextAction != 0;
        }
    }
    private float? lastActionTime;
    public float TimeUntilNextAction
    {
        get
        {
            if (lastActionTime is null)
            {
                return 0;
            }

            var timeUntilNextAction = (lastActionTime.Value + cooldown) - Time.time;
            if(timeUntilNextAction < 0)
            {
                timeUntilNextAction = 0;
            }
            return timeUntilNextAction;
        }
    }

    public bool CanDoAction()
    {
        return Waiting == false;
    }

    public void OnAction()
    {
        this.lastActionTime = Time.time;
    }
}
