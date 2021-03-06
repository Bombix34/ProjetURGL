﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(menuName = "actions data")]
public class ActionsData : ScriptableObject
{
    [SerializeField]
    private List<ActionPair> actionsData;

    public bool IsActionValid(TriggerTagType tag1, TriggerTagType tag2)
    {
        bool result = false;
        foreach (var action in actionsData)
        {
            if (action.IsTagPairedCorresponding(tag1, tag2))
                result = true;
        }
        return result;
    }
}

[System.Serializable]
public class ActionPair
{
    public TriggerTagType tag1, tag2;
    public ActionTypes concerningAction;

    public bool IsTagPairedCorresponding(TriggerTagType tagCompar1, TriggerTagType tagCompar2)
    {
        return tag1 == tagCompar1 && tag2 == tagCompar2 || tag1 == tagCompar2 && tag2 == tagCompar1;
    }
}
