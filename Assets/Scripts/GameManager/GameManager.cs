using Mirror;
using System.Linq;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance;
    [SyncVar]
    private GameState gameState;

    public GameState GameState { get => gameState; set => gameState = value; }

    private readonly SyncList<GameObject> aliveThieves = new SyncList<GameObject>();

    public bool AllowMovements
    {
        get
        {
            return this.gameState == GameState.PLAYING;
        }
    }


    private void Awake()
    {
        GameManager.Instance = this;
    }

    [Server]
    public void AddThief(GameObject player)
    {
        aliveThieves.Add(player);
    }

    [Server]
    public void RemoveThief(GameObject player)
    {
        aliveThieves.Remove(player);

        if (aliveThieves.Count == 0)
        {
            this.RpcEndGame(VictoryType.VIGILS_VICTORY);
        }
    }

    public GameObject GetNextThief(GameObject previousThief)
    {
        if (this.aliveThieves.Count == 0)
        {
            return null;
        }

        var index = this.aliveThieves.IndexOf(previousThief);
        index++;
        if (index >= this.aliveThieves.Count)
        {
            index = 0;
        }
        return this.aliveThieves.ElementAt(index);
    }

    [ServerCallback]
    public void EndGame(VictoryType victoryType)
    {
        this.gameState = GameState.END_GAME;
        this.RpcEndGame(victoryType);
    }

    [ClientRpc]
    private void RpcEndGame(VictoryType victoryType)
    {
        VictoryScreen.Instance.Init(victoryType);
    }
}
