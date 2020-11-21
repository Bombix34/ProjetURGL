using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomNetworkManagerHud : MonoBehaviour
{
    private NetworkManagerHUD networkManagerHUD;
    // Start is called before the first frame update
    void Start()
    {
        this.networkManagerHUD = GetComponent<NetworkManagerHUD>();
        SceneManager.sceneLoaded += this.OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "MenuScene")
        {
            this.networkManagerHUD.enabled = true;
            //this.networkManagerHUD.offsetX = 0;
            //this.networkManagerHUD.offsetY = 0;
        }
        else
        {
            this.networkManagerHUD.enabled = false;
            //this.networkManagerHUD.offsetX = 500;
            //this.networkManagerHUD.offsetY = -96;
        }
    }

}
