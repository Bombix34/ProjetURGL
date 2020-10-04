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
    public string name;
    public string description;
    public List<Quest> prepQuests;
    public Quest finalQuest;
    public MainQuestState mainQuestState = MainQuestState.PREP_QUEST;

    public string Name { get => name; private set => name = value; }
    public string Description { get => description; private set => description = value; }
    public List<Quest> PrepQuests { get => prepQuests; private set => prepQuests = value; }
    public Quest FinalQuest { get => finalQuest; private set => finalQuest = value; }
    public MainQuestState MainQuestState { get => mainQuestState; private set => mainQuestState = value; }
    public Action<MainQuestState> OnMainQuestStateChange { get; set; }
    public Action<Guid> OnFinishQuest { get; set; }
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
                if (this.PrepQuests.All(q => q.QuestState == QuestState.DONE))
                {
                    this.ChangeMainQuestState(MainQuestState.FINAL_QUEST);
                }
                break;
            case MainQuestState.FINAL_QUEST:
                if (this.FinalQuest.Guid != guid)
                {
                    throw new ArgumentException($"No quest for guid {guid}", nameof(guid));
                }

                this.FinalQuest.Finish();
                this.ChangeMainQuestState(MainQuestState.DONE);
                break;
        }
        this.OnFinishQuest(guid);
    }

    private void ChangeMainQuestState(MainQuestState newState)
    {
        this.MainQuestState = newState;
        this.OnMainQuestStateChange(this.MainQuestState);
    }
}
