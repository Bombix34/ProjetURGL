using UnityEngine;
using UnityEngine.UI;

public class RoomExitButtons : MonoBehaviour
{
    [SerializeField]
    [NotNull]
    private Button stopClientButton;
    [SerializeField]
    [NotNull]
    private Button stopHostButton;
    [SerializeField]
    [NotNull]
    private Button returnToRoomButton;

    public static RoomExitButtons Instance;
    private CustomNetworkRoomManager roomManager;

    private void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(Instance.gameObject);
        }

        Instance = this;

        this.stopClientButton.onClick.AddListener(this.StopClient);
        this.stopHostButton.onClick.AddListener(this.StopHost);
        this.returnToRoomButton.onClick.AddListener(this.ReturnToRoom);

        stopClientButton.gameObject.SetActive(false);
        stopHostButton.gameObject.SetActive(false);
        returnToRoomButton.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        if(Instance == this)
        {
            Instance = null;
        }
    }

    public void Init(CustomNetworkRoomManager roomManager, bool isHost = false)
    {
        this.roomManager = roomManager;

        if (isHost)
        {
            this.SetHostUI();
        }
        else
        {
            this.SetClientUI();
        }
    }

    private void SetClientUI()
    {
        stopClientButton.gameObject.SetActive(true);
        stopHostButton.gameObject.SetActive(false);
        returnToRoomButton.gameObject.SetActive(false);
    }

    private void SetHostUI()
    {
        stopClientButton.gameObject.SetActive(false);
        stopHostButton.gameObject.SetActive(true);
        returnToRoomButton.gameObject.SetActive(false);
    }

    public void EnterGame()
    {
        returnToRoomButton.gameObject.SetActive(true);
    }

    public void EnterRoom()
    {
        returnToRoomButton.gameObject.SetActive(false);
    }

    private void StopClient()
    {
        roomManager.StopClient();
    }

    private void StopHost()
    {
        roomManager.StopHost();
    }

    private void ReturnToRoom()
    {
        roomManager.ReturnToRoom();
    }
}
