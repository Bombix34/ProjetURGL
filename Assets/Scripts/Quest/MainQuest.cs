using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum MainQuestState
{
    PREP_QUEST,
    FINAL_QUEST,
    DONE
}
public class MainQuest : IMainQuest<Quest>
{
    public string Name { get; }
    public string Description { get; }
    public List<Quest> PrepQuests { get; }
    public Quest FinalQuest { get; }
    public MainQuestState MainQuestState { get; private set; } = MainQuestState.PREP_QUEST;
    public Action<MainQuestState> OnMainQuestStateChange { get; set; }
    public MainQuest()
    {

    }

    public MainQuest(string name, string description, List<Quest> prepQuests, Quest finalQuest)
    {
        Name = name;
        Description = description;
        PrepQuests = prepQuests;
        FinalQuest = finalQuest;
    }

    public void FinishQuest(Guid guid)
    {
        switch (MainQuestState)
        {
            case MainQuestState.DONE:
                throw new Exception($"The main quest is already done");
            case MainQuestState.PREP_QUEST:
                var quest = this.PrepQuests.Single(q => q.Guid == guid);
                quest.Finish();
                if(this.PrepQuests.All(q => q.QuestState == QuestState.DONE))
                {
                    this.ChangeMainQuestState(MainQuestState.FINAL_QUEST);
                }
                break;
            case MainQuestState.FINAL_QUEST:
                if(this.FinalQuest.Guid != guid)
                {
                    throw new ArgumentException($"No quest for guid {guid}", nameof(guid));
                }

                this.FinalQuest.Finish();
                this.ChangeMainQuestState(MainQuestState.DONE);
                break;
        }
    }

    private void ChangeMainQuestState(MainQuestState newState)
    {
        this.MainQuestState = newState;
        this.OnMainQuestStateChange(this.MainQuestState);
    }
}
