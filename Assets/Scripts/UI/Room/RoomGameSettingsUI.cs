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
    [SerializeField]
    [NotNull]
    private TMP_InputField cameraDistanceInput;
    [SerializeField]
    [NotNull]
    private PlayerSettingsUI thiefSettingsUI;
    [SerializeField]
    [NotNull]
    private PlayerSettingsUI vigilSettingsUI;
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
        InitCameraDistanceInput();
        gameSettingsPanel.SetActive(true);
        this.thiefSettingsUI.Init(RoomSettings.Instance.Settings.VoleurSettings);
        this.vigilSettingsUI.Init(RoomSettings.Instance.Settings.AgentSettings);
    }

    private void InitSceneDropdown()
    {
        sceneDropdown.ClearOptions();
        sceneDropdown.AddOptions(RoomSettings.Instance.Settings.GameScenes);
        sceneDropdown.onValueChanged.AddListener(ChangeGameScene);

        var sceneName = Path.GetFileNameWithoutExtension(this.roomManager.GameplayScene);
        var index = RoomSettings.Instance.Settings.GameScenes.IndexOf(sceneName);
        sceneDropdown.value = index;
    }
    public void ChangeGameScene(int index)
    {
        this.roomManager.GameplayScene = RoomSettings.Instance.Settings.GameScenes[index];
    }

    private void InitCameraDistanceInput()
    {
        cameraDistanceInput.text = RoomSettings.Instance.Settings.CameraDistance.ToString();
        cameraDistanceInput.onValueChanged.AddListener(OnCameraDistanceChange);
    }

    private void OnCameraDistanceChange(string speed)
    {
        RoomSettings.Instance.Settings.CameraDistance = float.Parse(speed.Replace('.', ','));
    }
}
