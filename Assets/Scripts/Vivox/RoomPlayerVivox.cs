using Mirror;
using System.Linq;
using UnityEngine;
using VivoxUnity;

public class RoomPlayerVivox : NetworkBehaviour
{
    public delegate void ChannelTextMessageReceived(string message);

    public static RoomPlayerVivox Instance;
    private bool connected = false;
    private string matchIdentifier="TODO";
    private VivoxVoiceManager vivoxVoiceManager;
    private RoomPlayerData roomPlayerData;
    public ChannelTextMessageReceived channelTextMessageReceived;

    private string roomChannelName => $"{matchIdentifier}-room";
    private string thiefChannelName => $"{matchIdentifier}-thief";
    private string vigilChannelName => $"{matchIdentifier}-vigil";

    // Start is called before the first frame update
    public override void OnStartLocalPlayer()
    {
        Instance = this;
        this.matchIdentifier = RoomSettings.Instance.roomUniqueIdentifier;
        this.vivoxVoiceManager = VivoxVoiceManager.Instance;
        this.roomPlayerData = GetComponent<RoomPlayerData>();

        vivoxVoiceManager.Login(this.roomPlayerData.GetPlayerIndentifier());
        vivoxVoiceManager.OnUserLoggedInEvent += OnLogin;
    }

    private void OnLogin()
    {
        this.connected = true;
        vivoxVoiceManager.OnTextMessageLogReceivedEvent += this.OnReceiveTextMessage;
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
        vivoxVoiceManager.JoinChannel(channelName, ChannelType.NonPositional, VivoxVoiceManager.ChatCapability.TextAndAudio);
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
