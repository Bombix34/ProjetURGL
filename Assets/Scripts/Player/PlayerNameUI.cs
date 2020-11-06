using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNameUI : MonoBehaviour
{
    [SerializeField]
    private Text textUi;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(nameof(SetName));
    }

    IEnumerator SetName()
    {
        var playerManager = GetComponent<PlayerManager>();
        yield return new WaitUntil(() => PlayerManager.localPlayer != null);
        if (playerManager.PlayerType == PlayerType.THIEF && PlayerManager.localPlayer.PlayerType == PlayerType.VIGIL)
        {
            yield break;
        }
        textUi.gameObject.SetActive(true);
        textUi.text = playerManager.PlayerName;
    }

}
