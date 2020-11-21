using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomPlayerUI : MonoBehaviour
{
    [SerializeField]
    [NotNull]
    private TextMeshProUGUI playerNameText;
    [SerializeField]
    [NotNull]
    private TMP_Dropdown playerTypeDropdown;
    [SerializeField]
    [NotNull]
    private Toggle playerReadyToggle;

    private CustomNetworkRoomPlayer customNetworkRoomPlayer;
    private bool isLocalPlayer;

    public void Init(CustomNetworkRoomPlayer customNetworkRoomPlayer)
    {
        this.customNetworkRoomPlayer = customNetworkRoomPlayer;
        this.customNetworkRoomPlayer.onReadyStateChange += OnReadyStateChange;
        this.customNetworkRoomPlayer.RoomPlayerData.onPlayerTypeChange += OnChangePlayerType;
        isLocalPlayer = customNetworkRoomPlayer.isLocalPlayer;
        if (!isLocalPlayer)
        {
            this.playerTypeDropdown.interactable = false;
            this.playerReadyToggle.interactable = false;
        }
        this.playerNameText.text = customNetworkRoomPlayer.RoomPlayerData.PlayerIndentifier;
        this.playerTypeDropdown.value = (int)customNetworkRoomPlayer.RoomPlayerData.PlayerType;
        this.playerReadyToggle.isOn = customNetworkRoomPlayer.readyToBegin;
    }

    private void OnReadyStateChange(bool newState)
    {
        playerReadyToggle.isOn = newState;
    }

    public void ChangeReadyState()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        customNetworkRoomPlayer.CmdChangeReadyState(playerReadyToggle.isOn);
    }
    public void OnChangePlayerType(PlayerType playerType)
    {
        this.playerTypeDropdown.value = (int)playerType;
    }

    public void ChangePlayerType()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        var playerType = (PlayerType)this.playerTypeDropdown.value;
        customNetworkRoomPlayer.RoomPlayerData.CmdChangePlayerType(playerType);
    }
}
