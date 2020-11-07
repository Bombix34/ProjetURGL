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

    public bool CanDoAction(ActionTypes type)
    {
        return configurations.Any(q => q.ActionType == type && q.CanDoAction());
    }
    public void DoAction(ActionTypes type)
    {
        configurations.SingleOrDefault(q => q.ActionType == type && q.CanDoAction()).OnAction();
    }
    public ActionConfiguration GetAction(ActionTypes type)
    {
        return configurations.SingleOrDefault(q => q.ActionType == type);
    }
}
