using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestPanel : MonoBehaviour
{
    private MainQuest quest;
    [SerializeField]
    private TextMeshProUGUI QuestName;
    [SerializeField]
    private TextMeshProUGUI QuestDescription;
    [SerializeField]
    private GameObject goalPanelPrefab;
    private Dictionary<Guid, GameObject> guidToGoalPanelDict = new Dictionary<Guid, GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        this.quest = QuestManager.Instance.MainQuest;

        this.QuestName.text = this.quest.Name;
        this.QuestDescription.text = this.quest.Description;
        foreach (var goal in this.quest.PrepQuests)
        {
            this.InstantiateGoalUI(goal);
        }

        this.quest.OnMainQuestStateChange += OnChangeQuestState;
        this.quest.OnFinishQuest += this.DestroyGoalPanelByGuid;
    }
    private void OnChangeQuestState(MainQuestState mainQuestState)
    {
        switch (mainQuestState)
        {
            case MainQuestState.FINAL_QUEST:
                this.InstantiateGoalUI(this.quest.FinalQuest);
                break;
            default:
                break;
        }
    }

    private void InstantiateGoalUI(Quest goal)
    {
        var goalPanelGameObject = Instantiate(goalPanelPrefab, this.transform);
        goalPanelGameObject.GetComponent<GoalPanel>().Goal = goal;
        guidToGoalPanelDict.Add(goal.Guid, goalPanelGameObject);
    }

    private void DestroyGoalPanelByGuid(Guid guid)
    {
        this.guidToGoalPanelDict[guid].GetComponent<GoalPanel>().Finish();
        this.guidToGoalPanelDict.Remove(guid);
    }
}
