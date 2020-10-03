using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New MainQuest", menuName = "Quest/Main Quest")]
public class MainQuestScriptableObject : ScriptableObject, IMainQuest<QuestScriptableObject>
{
    [SerializeField]
    private string questName;
    [SerializeField]
    private string description;
    [SerializeField]
    private List<QuestScriptableObject> prepQuests;
    [SerializeField]
    private QuestScriptableObject finalQuest;

    public string Name { get => questName; private set => questName = value; }
    public string Description { get => description; private set => description = value; }
    public List<QuestScriptableObject> PrepQuests { get => prepQuests; private set => prepQuests = value; }
    public QuestScriptableObject FinalQuest { get => finalQuest; private set => finalQuest = value; }
}
