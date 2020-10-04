using UnityEngine;

public interface IQuest
{
    string Description { get; }
    float Duration { get; }
    string Name { get; }
    Vector2 Position { get; }
    float Radius { get; }
}