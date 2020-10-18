using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VictoryScreen : Singleton<VictoryScreen>
{
    public enum VictoryType
    {
        THIEFS_VICTORY,
        VIGILS_VICTORY
    }
    const string THIEFS_VICTORY = "Les voleurs ont gagnés";
    const string VIGILS_VICTORY = "Les vigiles ont gagnés";
    [SerializeField]
    private GameObject panel = null;
    [SerializeField]
    private TextMeshProUGUI textMeshProUGUI = null;

    public void Init(VictoryType victoryType)
    {
        switch (victoryType)
        {
            case VictoryType.THIEFS_VICTORY:
                this.textMeshProUGUI.text = THIEFS_VICTORY;
                break;
            case VictoryType.VIGILS_VICTORY:
                this.textMeshProUGUI.text = VIGILS_VICTORY;
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
