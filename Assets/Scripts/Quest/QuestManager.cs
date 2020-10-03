using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : Singleton<QuestManager>
{
    [SerializeField]
    private MainQuestScriptableObject mainQuestScriptableObject;
    [SerializeField]
    private GameObject questAreaPrefab;

    public MainQuest MainQuest { get; private set; }
    public MainQuestScriptableObject MainQuestScriptableObject { get => mainQuestScriptableObject; private set => mainQuestScriptableObject = value; }
    void Start()
    {
        this.MainQuest = MainQuestScriptableObjectToMainQuestMapper.Map(MainQuestScriptableObject);

        foreach (var quest in MainQuest.PrepQuests)
        {
            this.InstantiateQuestArea(quest);
        }
        this.InstantiateQuestArea(this.MainQuest.FinalQuest);
        //Debug.Log(this.MainQuest.Name);
        //Debug.Log(this.MainQuest.PrepQuests.Count);
        //Debug.Log(this.MainQuest.FinalQuest.Name);

        //Debug.Log("--------");
        //Debug.Log(this.MainQuest.MainQuestState);
        //Debug.Log("--------");
        //this.FinishQuest(this.MainQuest.PrepQuests[0].Guid);
        //Debug.Log(this.MainQuest.MainQuestState);
        //Debug.Log("--------");
        //this.FinishQuest(this.MainQuest.PrepQuests[1].Guid);
        //Debug.Log(this.MainQuest.MainQuestState);
        //Debug.Log("--------");
        //this.FinishQuest(this.MainQuest.FinalQuest.Guid);
        //Debug.Log(this.MainQuest.MainQuestState);
        //Debug.Log("--------");
    }

    private void InstantiateQuestArea(Quest quest)
    {
        var questAreaGameObject = Instantiate(questAreaPrefab);
        questAreaGameObject.GetComponent<QuestArea>().Quest = quest;
    }

    public void FinishQuest(Guid guid)
    {
        this.MainQuest.FinishQuest(guid);
    }
}
