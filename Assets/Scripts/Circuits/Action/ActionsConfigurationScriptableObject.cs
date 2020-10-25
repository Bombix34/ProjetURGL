using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "new ActionsConfiguration", menuName = "URGL/ActionsConfiguration")]
public class ActionsConfigurationScriptableObject : ScriptableObject
{
    [SerializeField]
    private List<ActionConfiguration> configurations = new List<ActionConfiguration>();

    public List<ActionConfiguration> Configurations => configurations;

    public WaitCoroutine GetActionWaitCoroutine(ActionTypes type)
    {
        return configurations.Single(q => q.ActionType == type && !q.WaitCoroutine.IsWaiting).WaitCoroutine;
    }
    public bool CanDoAction(ActionTypes type)
    {
        return configurations.Any(q => q.ActionType == type && !q.WaitCoroutine.IsWaiting);
    }
}
