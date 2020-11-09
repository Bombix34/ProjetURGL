using System;
using UnityEngine;

[Serializable]
public class WaitingActionConfigurationParameter : BaseActionConfigurationParameter
{
    [SerializeField]
    private float cooldown = 0;
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
            if (timeUntilNextAction < 0)
            {
                timeUntilNextAction = 0;
            }
            return timeUntilNextAction;
        }
    }

    protected override bool CanDoActionIfEnabled()
    {
        return Waiting == false;
    }

    protected override void OnActionIfEnabled()
    {
        this.lastActionTime = Time.time;
    }
}
