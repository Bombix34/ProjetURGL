using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FollowPlayerCameraMovements : GameCameraMovements
{
    private readonly List<PlayerManager> players;
    private int index = 0;
    private bool arePlayersAlive;

    public FollowPlayerCameraMovements(Transform transform, float smoothness, float offsetZ, PlayerType? playerType = null) : base(transform, smoothness, offsetZ)
    {
        if (playerType.HasValue)
        {
            switch (playerType.Value)
            {
                case PlayerType.THIEF:
                    this.players = UnityEngine.Object.FindObjectsOfType<ThiefManager>().Cast<PlayerManager>().ToList();
                    break;
                case PlayerType.VIGIL:
                    this.players = UnityEngine.Object.FindObjectsOfType<VigilManager>().Cast<PlayerManager>().ToList();
                    break;
                case PlayerType.SPECTATOR:
                default:
                    throw new NotImplementedException($"The value {playerType.Value} is not implemented");
            }
        }
        else
        {
            this.players = UnityEngine.Object.FindObjectsOfType<PlayerManager>().ToList();
        }
        foreach (var player in this.players)
        {
            player.onAliveChange += RemovePlayer;
        }
        this.playerTransform = players.FirstOrDefault(p => p.Alive)?.transform;
        this.arePlayersAlive = playerTransform != null;
    }

    public override void Update()
    {
        base.Update();
        if (!arePlayersAlive)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            this.PreviousPlayer();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            this.NextPlayer();
        }
    }

    private void RemovePlayer(PlayerManager playerManager)
    {
        this.players.Remove(playerManager);
        if(this.players.All(p => !p.Alive))
        {
            this.playerTransform = null;
            this.arePlayersAlive = false;
            return;
        }
        if(this.playerTransform == playerManager.transform)
        {
            this.NextPlayer();
        }
    }

    private bool TrySetPlayer()
    {
        if(index >= this.players.Count)
        {
            return false;
        }
        var currentPlayer = this.players[index];
        if (currentPlayer.Alive)
        {
            this.playerTransform = currentPlayer.transform;
            return true;
        }
        return false;
    }

    private void PreviousPlayer()
    {
        index--;
        if (index < 0)
        {
            index = this.players.Count - 1;
        }

        if (!this.TrySetPlayer())
        {
            PreviousPlayer();
        }
    }
    private void NextPlayer()
    {
        index++;
        if (index >= this.players.Count)
        {
            index = 0;
        }

        if (!this.TrySetPlayer())
        {
            NextPlayer();
        }
    }
}
