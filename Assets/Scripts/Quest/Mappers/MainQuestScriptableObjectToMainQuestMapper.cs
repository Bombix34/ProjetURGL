using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MainQuestScriptableObjectToMainQuestMapper
{
    public static MainQuest Map(MainQuestScriptableObject mainQuestScriptableObject)
    {
        var finalQuest = QuestScriptableObjectToQuestMapper.Map(mainQuestScriptableObject.FinalQuest);
        var prepQuests = mainQuestScriptableObject.PrepQuests.Select(questScriptableObject => QuestScriptableObjectToQuestMapper.Map(questScriptableObject)).ToList();
        return new MainQuest(mainQuestScriptableObject.Name, mainQuestScriptableObject.Description, prepQuests, finalQuest);
    }
}
