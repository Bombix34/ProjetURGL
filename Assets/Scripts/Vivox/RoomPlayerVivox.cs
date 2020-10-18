using Mirror;
using System;
using System.Linq;
using UnityEngine;
using VivoxUnity;

public class RoomPlayerVivox : NetworkBehaviour
{
    public delegate void ChannelTextMessageReceived(string message);

    public static RoomPlayerVivox Instance;
    private string matchIdentifier="TODO";
    private VivoxVoiceManager vivoxVoiceManager;
    private RoomPlayerData roomPlayerData;
    public ChannelTextMessageReceived channelTextMessageReceived;

    private string RoomChannelName => $"{matchIdentifier}-room";
    private string ThiefChannelName => $"{matchIdentifier}-thief";
    private string VigilChannelName => $"{matchIdentifier}-vigil";
    private IChannelSession currentChannelSession;

    // Start is called before the first frame update
    public override void OnStartLocalPlayer()
    {
        if(Instance != null)
        {
            this.JoinRoomChannel();
            return;
        }
        Instance = this;
        this.matchIdentifier = RoomSettings.Instance.roomUniqueIdentifier;
        this.vivoxVoiceManager = VivoxVoiceManager.Instance;
        this.roomPlayerData = GetComponent<RoomPlayerData>();

        vivoxVoiceManager.Login(this.roomPlayerData.PlayerIndentifier);
        vivoxVoiceManager.OnUserLoggedInEvent += OnLogin;
    }

    private void OnLogin()
    {
        vivoxVoiceManager.OnTextMessageLogReceivedEvent += this.OnReceiveTextMessage;
        this.JoinRoomChannel();
    }

    private void JoinRoomChannel()
    {
        this.JoinChannel(this.RoomChannelName);
    }

    public void StartGame(PlayerType playerType)
    {
        switch (playerType)
        {
            case PlayerType.THIEF:
                this.JoinThiefChannel();
                break;
            case PlayerType.VIGIL:
                this.JoinVigilChannel();
                break;
            case PlayerType.SPECTATOR:
                this.JoinSpectatorChannel();
                break;
            default:
                throw new NotImplementedException($"The value {playerType} is not implemented");
        }
    }

    private void JoinThiefChannel()
    {
        print("JoinThiefChannel");
        this.JoinChannel(this.ThiefChannelName);
    }

    private void JoinVigilChannel()
    {
        print("JoinVigilChannel");
        this.JoinChannel(this.VigilChannelName);
    }

    private void JoinSpectatorChannel()
    {
        this.JoinChannel(this.ThiefChannelName);
    }

    private void JoinChannel(string channelName)
    {
        if(currentChannelSession != null)
        {
            currentChannelSession.Disconnect();
        }
        currentChannelSession = vivoxVoiceManager.JoinChannel(channelName, ChannelType.NonPositional, VivoxVoiceManager.ChatCapability.TextAndAudio);
    }

    public void OnReceiveTextMessage(string sender, IChannelTextMessage channelTextMessage)
    {
        Debug.LogWarning(channelTextMessage.Message);
        channelTextMessageReceived?.Invoke($"{sender}: {channelTextMessage.Message}");
    }

    public void SendChatMessage(string message)
    {
        vivoxVoiceManager.SendTextMessage(message, vivoxVoiceManager.ActiveChannels.First().Key);
    }
}
