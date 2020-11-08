using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNameUI : MonoBehaviour
{
    [SerializeField]
    [NotNull]
    private Text textUi = null;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(nameof(SetName));
    }

    IEnumerator SetName()
    {
        var playerManager = GetComponent<PlayerManager>();
        yield return new WaitUntil(() => PlayerManager.localPlayer != null);
        if (PlayerManager.localPlayer.PlayerType != playerManager.PlayerType)
        {
            yield break;
        }
        textUi.gameObject.SetActive(true);
        textUi.text = playerManager.PlayerName;
    }

}
