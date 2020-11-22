using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class RoomGameSettingsUI : MonoBehaviour
{
    [SerializeField]
    [NotNull]
    private GameObject gameSettingsPanel;
    [SerializeField]
    [NotNull]
    private TMP_Dropdown sceneDropdown;
    private CustomNetworkRoomManager roomManager;
    public static RoomGameSettingsUI Instance;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        gameSettingsPanel.SetActive(false);
    }
    
    public void Init(CustomNetworkRoomManager customNetworkRoomManager)
    {
        this.roomManager = customNetworkRoomManager;
        InitSceneDropdown();
        gameSettingsPanel.SetActive(true);
    }

    private void InitSceneDropdown()
    {
        sceneDropdown.ClearOptions();
        sceneDropdown.AddOptions(this.roomManager.Settings.GameScenes);
        sceneDropdown.onValueChanged.AddListener(ChangeGameScene);

        var sceneName = Path.GetFileNameWithoutExtension(this.roomManager.GameplayScene);
        var index = this.roomManager.Settings.GameScenes.IndexOf(sceneName);
        sceneDropdown.value = index;
    }
    public void ChangeGameScene(int index)
    {
        this.roomManager.GameplayScene = this.roomManager.Settings.GameScenes[index];
    }
}
