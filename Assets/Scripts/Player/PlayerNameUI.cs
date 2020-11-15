using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNameUI : MonoBehaviour
{
    [SerializeField]
    [NotNull]
    private Text textUi = null;

    private bool canBeActivated;

    void Start()
    {

        var playerManager = GetComponent<PlayerManager>();
        textUi.text = playerManager.PlayerName;

        switch (RoomPlayerData.LocalPlayer.PlayerType)
        {
            case PlayerType.THIEF:
            case PlayerType.SPECTATOR:
                canBeActivated = true;
                break;
            case PlayerType.VIGIL:
                canBeActivated = playerManager.PlayerType == PlayerType.VIGIL;
                break;
            default:
                throw new NotImplementedException($"The value {RoomPlayerData.LocalPlayer.PlayerType} is not implemented");
        }

        if(playerManager.PlayerName == RoomPlayerData.LocalPlayer.PlayerIndentifier)
        {
            this.DisplayName(true);
        }
    }


    public void DisplayName(bool active)
    {
        if (!canBeActivated)
        {
            return;
        }
        textUi.gameObject.SetActive(active);
    }

}
