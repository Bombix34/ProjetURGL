using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomPlayerContainerUI : MonoBehaviour
{
    public static RoomPlayerContainerUI Instance;
    private RectTransform rectTransform;
    private readonly List<CustomNetworkRoomPlayer> players = new List<CustomNetworkRoomPlayer>();
    private readonly Dictionary<CustomNetworkRoomPlayer, GameObject> playerToUiGameObjectDict = new Dictionary<CustomNetworkRoomPlayer, GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        rectTransform = GetComponent<RectTransform>();
    }

    public void AddPlayer(CustomNetworkRoomPlayer player)
    {
        if (this.players.Contains(player))
        {
            return;
        }
        this.players.Add(player);
        var roomPlayerUIPrefab = Resources.Load<GameObject>("UI/Room/RoomPlayerPanel");
        var roomPlayerUIGameObject = Instantiate(roomPlayerUIPrefab, transform);
        roomPlayerUIGameObject.GetComponent<RoomPlayerUI>().Init(player);
        LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
        this.playerToUiGameObjectDict.Add(player, roomPlayerUIGameObject);
    }

    public void RemovePlayer(CustomNetworkRoomPlayer player)
    {
        if (!this.players.Contains(player))
        {
            return;
        }
        this.players.Remove(player);
        Destroy(this.playerToUiGameObjectDict[player]);
        this.playerToUiGameObjectDict.Remove(player);
        LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
    }
}
