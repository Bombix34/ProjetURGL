using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "Quest/Simple Quest")]
public class QuestScriptableObject : ScriptableObject, IQuest
{
    [SerializeField]
    private string questName;
    [SerializeField]
    private string description;
    [SerializeField]
    private float duration;
    [SerializeField]
    private Vector2 position;
    [SerializeField]
    private float radius;

    public string Name { get => questName; private set => questName = value; }
    public string Description { get => description; private set => description = value; }
    public float Duration { get => duration; private set => duration = value; }
    public Vector2 Position { get => position; private set => position = value; }
    public float Radius { get => radius; private set => radius = value; }
}
