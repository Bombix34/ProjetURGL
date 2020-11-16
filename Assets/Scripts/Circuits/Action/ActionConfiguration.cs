using System;
using UnityEngine;

[Serializable]
public class ActionConfiguration
{
    [SerializeField]
    private ActionTypes actionType = ActionTypes.NOT_SET;
    [SerializeField]
    private WaitingActionConfigurationParameter waitingActionConfigurationParameter = null;
    [SerializeField]
    private ChargeActionConfigurationParameter chargeActionConfigurationParameter = null;

    public ActionTypes ActionType => actionType;

    public void Init()
    {
        this.waitingActionConfigurationParameter.Init();
        this.chargeActionConfigurationParameter.Init();
    }

    public bool CanDoAction()
    {
        return this.waitingActionConfigurationParameter.CanDoAction() 
            && this.chargeActionConfigurationParameter.CanDoAction();
    }

    public void OnAction()
    {
        this.waitingActionConfigurationParameter.OnAction();
        this.chargeActionConfigurationParameter.OnAction();
    }
}
