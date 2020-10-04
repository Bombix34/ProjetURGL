using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class QuestScriptableObjectToQuestMapper
{
    public static Quest Map(QuestScriptableObject questScriptableObject)
    {
        return new Quest(questScriptableObject.Name, questScriptableObject.Description, questScriptableObject.Duration, questScriptableObject.Position, questScriptableObject.Radius);
    }
}
