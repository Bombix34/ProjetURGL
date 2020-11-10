using Mirror;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingManager : NetworkBehaviour
{
    public static LoadingManager Instance;

    [SerializeField]
    private int playerLoaded = 0;

    [SerializeField]
    [NotNull]
    private GameObject loadingPanel = null;

    void Start()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Enable()
    {
        loadingPanel.SetActive(true);
    }

    private void Disable()
    {
        loadingPanel.SetActive(false);
    }

    [ClientRpc]
    private void RpcDisable()
    {
        Disable();
    }

    public void ResetPlayerLoadedCounter()
    {
        this.playerLoaded = 0;
    }

    [Command(ignoreAuthority = true)]
    public void CmdPlayerReady()
    {
        this.playerLoaded++;

        if (playerLoaded == NetworkManager.singleton.numPlayers)
        {
            RpcDisable();
            GameManager.Instance.RpcStartIntroduction();
        }
    }
}
