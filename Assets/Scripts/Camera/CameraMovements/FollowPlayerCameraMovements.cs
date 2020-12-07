using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FollowPlayerCameraMovements : GameCameraMovements
{
    private readonly List<PlayerManager> players;
    private int index = 0;
    private bool arePlayersAlive = true;
    private readonly InteractionZone interactionZone;

    public FollowPlayerCameraMovements(Transform transform, float smoothness, float offsetZ, PlayerType? playerType = null) : base(transform, smoothness, offsetZ)
    {
        this.interactionZone = Camera.main.GetComponentInChildren<InteractionZone>();
        switch (playerType)
        {
            case PlayerType.THIEF:
                this.players = UnityEngine.Object.FindObjectsOfType<ThiefManager>().Cast<PlayerManager>().ToList();
                break;
            case PlayerType.VIGIL:
                this.players = UnityEngine.Object.FindObjectsOfType<VigilManager>().Cast<PlayerManager>().ToList();
                break;
            case PlayerType.SPECTATOR:
            default:
                this.players = UnityEngine.Object.FindObjectsOfType<PlayerManager>().ToList();
                break;
        }

        foreach (var player in this.players)
        {
            player.onAliveChange += RemovePlayer;
        }

        var firstAlivePlayer = players.FirstOrDefault(p => p.Alive);
        if(firstAlivePlayer is null)
        {
            this.arePlayersAlive = false;
            this.playerTransform = null;
            return;
        }
        this.index = this.players.IndexOf(firstAlivePlayer);
        this.SetPlayer(firstAlivePlayer);
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
            SetPlayer(currentPlayer);
            return true;
        }
        return false;
    }

    private void SetPlayer(PlayerManager currentPlayer)
    {
        this.ChangePlayerTransform(currentPlayer.transform);
        this.interactionZone.SwitchPlayer(currentPlayer);
        InventoryUI.Instance.ChangeInventory(currentPlayer.GetComponent<Inventory>());
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
