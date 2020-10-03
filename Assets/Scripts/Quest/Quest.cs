using System;
using UnityEngine;

public enum QuestState
{
    TODO,
    DOING,
    DONE
}

public class Quest: IQuest
{
    public Guid Guid { get; } = Guid.NewGuid();
    public string Name { get; private set; }
    public string Description { get; private set; }
    public QuestState QuestState { get; private set; } = QuestState.TODO;
    public float Duration { get; private set; }
    public Vector2 Position { get; private set; }
    public float Radius { get; private set; }

    public Quest(string name, string description, float duration, Vector2 position, float radius)
    {
        Name = name;
        Description = description;
        Duration = duration;
        Position = position;
        Radius = radius;
    }

    public void Finish()
    {
        this.QuestState = QuestState.DONE;
    }
}
