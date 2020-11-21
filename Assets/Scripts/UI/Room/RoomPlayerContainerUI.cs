using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomPlayerContainerUI : MonoBehaviour
{
    public static RoomPlayerContainerUI Instance;
    private RectTransform rectTransform;
    private readonly List<CustomNetworkRoomPlayer> players = new List<CustomNetworkRoomPlayer>();

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
    }
}
