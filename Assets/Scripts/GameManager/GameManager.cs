using Mirror;
using System.Linq;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance;
    [SyncVar]
    private GameState gameState = GameState.LOADING;

    public GameState GameState { get => gameState; set => gameState = value; }

    private readonly SyncList<GameObject> aliveThieves = new SyncList<GameObject>();

    private PNJPoolManager pnjsManager;

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
        pnjsManager = GetComponent<PNJPoolManager>();
    }

    [ClientRpc]
    public void RpcStartIntroduction()
    {
        this.GameState = GameState.INTRODUCTION;
        if(isServer)
            pnjsManager.InitPNJ();
        CameraManager.Instance.StartIntro(OnEndIntroduction);
    }

    [ServerCallback]
    public void OnEndIntroduction()
    {
        pnjsManager.LaunchGame();
        this.RpcStartPlaying();
    }
    
    [ClientRpc]
    public void RpcStartPlaying()
    {
        this.GameState = GameState.PLAYING;
        CameraManager.Instance.StartPlaying();
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
        pnjsManager.StopAllPNJ();
        this.RpcEndGame(victoryType);
    }

    [ClientRpc]
    private void RpcEndGame(VictoryType victoryType)
    {
        VictoryScreen.Instance.Init(victoryType);
    }
}
