using System.Collections.Generic;

public interface IMainQuest<TQuest> where TQuest: IQuest
{
    string Description { get; }
    TQuest FinalQuest { get; }
    string Name { get; }
    List<TQuest> PrepQuests { get; }
}