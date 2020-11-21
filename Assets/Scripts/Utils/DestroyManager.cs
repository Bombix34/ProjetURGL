using UnityEngine;
using UnityEngine.SceneManagement;

public class DestroyManager : MonoBehaviour
{
    [SerializeField]
    [NotNull]
    private string sceneName = null;

    private static bool instantiated = false;

    void Start()
    {
        if (instantiated)
        {
            DestroyImmediate(gameObject);
            return;
        }
        instantiated = true;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(SceneComparer.Compare(scene.name, sceneName))
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        instantiated = false;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
