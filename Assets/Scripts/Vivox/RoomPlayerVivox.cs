using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomPlayerVivox : NetworkBehaviour
{
    private bool connected = false;
    private string matchIdentifier="TODO";
    private VivoxVoiceManager vivoxVoiceManager;
    private RoomPlayerData roomPlayerData;

    private string roomChannelName => $"{matchIdentifier}-room";
    private string thiefChannelName => $"{matchIdentifier}-thief";
    private string vigilChannelName => $"{matchIdentifier}-vigil";

    // Start is called before the first frame update
    public override void OnStartLocalPlayer()
    {
        this.matchIdentifier = RoomSettings.Instance.roomUniqueIdentifier;
        this.vivoxVoiceManager = VivoxVoiceManager.Instance;
        this.roomPlayerData = GetComponent<RoomPlayerData>();

        vivoxVoiceManager.Login(this.roomPlayerData.GetPlayerIndentifier());
        vivoxVoiceManager.OnUserLoggedInEvent += OnLogin;
    }

    private void OnLogin()
    {
        this.connected = true;
        this.JoinRoomChannel();
    }

    private void JoinRoomChannel()
    {
        this.JoinChannel(this.roomChannelName);
    }

    private void JoinThiefChannel()
    {
        this.JoinChannel(this.thiefChannelName);
    }

    private void JoinVigilChannel()
    {
        this.JoinChannel(this.vigilChannelName);
    }

    private void JoinChannel(string channelName)
    {
        vivoxVoiceManager.JoinChannel(channelName, VivoxUnity.ChannelType.NonPositional, VivoxVoiceManager.ChatCapability.TextAndAudio);
    }

    private void OnDestroy()
    {
        if (this.connected)
        {
            //vivoxVoiceManager.DisconnectAllChannels();
            //WARNING unity crash with this line sometimes (maybe fix with the if)...
            //vivoxVoiceManager.Logout();
        }
    }
}
