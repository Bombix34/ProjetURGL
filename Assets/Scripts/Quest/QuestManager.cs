using Mirror;
using System;
using UnityEngine;

public class QuestManager : NetworkBehaviour
{
    [SerializeField]
    private MainQuestScriptableObject mainQuestScriptableObject;
    [SerializeField]
    private GameObject questAreaPrefab;

    [SyncVar]
    private MainQuest mainQuest;
    public MainQuest MainQuest { get => mainQuest; private set => mainQuest = value; }
    public MainQuestScriptableObject MainQuestScriptableObject { get => mainQuestScriptableObject; private set => mainQuestScriptableObject = value; }
    public static QuestManager Instance { get; private set; }

    public override void OnStartServer()
    {
        base.OnStartServer();
        Instance = this;

        if (!isServer)
        {
            return;
        }
        this.MainQuest = MainQuestScriptableObjectToMainQuestMapper.Map(MainQuestScriptableObject);
        this.MainQuest.OnMainQuestStateChange += this.OnChangeQuestState;

        foreach (var quest in MainQuest.PrepQuests)
        {
            this.InstantiateQuestArea(quest);
        }
    }

    private void OnChangeQuestState(MainQuestState mainQuestState)
    {
        switch (mainQuestState)
        {
            case MainQuestState.FINAL_QUEST:
                this.InstantiateQuestArea(this.mainQuest.FinalQuest);
                break;
            case MainQuestState.DONE:
                Debug.Log("VICTORY");
                break;
            default:
                break;
        }
    }

    [ServerCallback]
    private void InstantiateQuestArea(Quest quest)
    {
        Debug.Log(quest.Radius);
        var questAreaGameObject = Instantiate(questAreaPrefab);
        questAreaGameObject.GetComponent<QuestArea>().Quest = quest;
        NetworkServer.Spawn(questAreaGameObject);
    }

    [ClientRpc]
    public void RpcFinishQuest(Guid guid)
    {
        this.MainQuest.FinishQuest(guid);
    }
}
