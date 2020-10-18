using TMPro;
using UnityEngine;

public class ChatDisplay : MonoBehaviour
{
    public GameObject ChatMessagePrefab;
    public Transform MessageTransformParent;
    private bool initialized = false;


    private void Update()
    {
        if (!initialized)
        {
            if(RoomPlayerVivox.Instance != null)
            {
                RoomPlayerVivox.Instance.channelTextMessageReceived += OnMessageReceived;
                initialized = true;
            }
        }
    }

    private void OnMessageReceived(string message)
    {
        var messageGameobject = Instantiate(ChatMessagePrefab, MessageTransformParent);
        messageGameobject.GetComponent<TextMeshProUGUI>().text = message;
    }
}
