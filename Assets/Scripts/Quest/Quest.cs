using System;
using UnityEngine;

public enum QuestState
{
    TODO,
    DOING,
    DONE
}

public class Quest : IQuest
{
    public Guid guid = Guid.NewGuid();
    public string name;
    public string description;
    public QuestState questState = QuestState.TODO;
    public float duration;
    public Vector2 position;
    public float radius;

    public Guid Guid { get => guid; private set => guid = value; }
    public string Name { get => name; private set => name = value; }
    public string Description { get => description; private set => description = value; }
    public QuestState QuestState { get => questState; private set => questState = value; }
    public float Duration { get => duration; private set => duration = value; }
    public Vector2 Position { get => position; private set => position = value; }
    public float Radius { get => radius; private set => radius = value; }

    public Quest()
    {

    }

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
