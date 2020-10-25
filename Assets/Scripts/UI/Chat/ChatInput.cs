using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChatInput : MonoBehaviour
{
    private TMP_InputField inputField;
    // Start is called before the first frame update
    void Start()
    {
        inputField = GetComponent<TMP_InputField>();
        inputField.onSubmit.AddListener(SubmitMessage);
    }

    private void SubmitMessage(string message)
    {
        RoomPlayerVivox.Instance.SendChatMessage(message);
    }
}
