using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VictoryScreen : Singleton<VictoryScreen>
{
    public enum VictoryType
    {
        STUDENT_VICTORY,
        TEACHER_VICTORY
    }
    const string STUDENT_VICTORY = "Les enfants ont gagnés";
    const string TEACHER_VICTORY = "Les enseignants ont gagnés";
    [SerializeField]
    private GameObject panel;
    [SerializeField]
    private TextMeshProUGUI textMeshProUGUI;

    public void Init(VictoryType victoryType)
    {
        switch (victoryType)
        {
            case VictoryType.STUDENT_VICTORY:
                this.textMeshProUGUI.text = STUDENT_VICTORY;
                break;
            case VictoryType.TEACHER_VICTORY:
                this.textMeshProUGUI.text = TEACHER_VICTORY;
                break;
        }
        panel.SetActive(true);
        Invoke(nameof(ReturnToRoom), 3);
    }

    private void ReturnToRoom()
    {
        var networkRoomManager = FindObjectOfType<NetworkRoomManager>();
        networkRoomManager.ServerChangeScene(networkRoomManager.RoomScene);
    }

}
