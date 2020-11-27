using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;

[System.Serializable]
public class GameResult
{
    private const string RESULT_PATH = "results.csv";
    public string Scene { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;
    public VictoryType VictoryType { get; set; }
    public List<GameResultPlayer> Players { get; set; }
    public GameResult(VictoryType victoryType, List<GameResultPlayer> players, string scene)
    {
        VictoryType = victoryType;
        Players = players;
        Scene = scene;
    }

    public void SaveResult()
    {
        if (!File.Exists(RESULT_PATH))
        {
            File.AppendAllText(RESULT_PATH, "Date, Scene, VictoryType, Thieves, Vigils\n");
        }
        File.AppendAllText(RESULT_PATH, ToCsv());
    }

    private string ToCsv()
    {
        var playerTypePlayers = Players.GroupBy(q => q.PlayerType).ToDictionary(q => q.Key, q => q.Select(p => p.PlayerName).ToArray());
        var thieves = playerTypePlayers.ContainsKey(PlayerType.THIEF) ? string.Join(", ", playerTypePlayers[PlayerType.THIEF].ToArray()) : "";
        var vigils = playerTypePlayers.ContainsKey(PlayerType.VIGIL) ? string.Join(", ", playerTypePlayers[PlayerType.VIGIL].ToArray()) : "";
        var result = $"{Date:dd MMMM yyyy HH:mm}, {Scene}, {VictoryType}, {thieves}, {vigils}\n";
        return result;
    }

}
