using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class LoadMenuScene : EditorWindow
{
    [MenuItem("FafaStudio/Switch Menu Scene", priority = 1)]
    public static void LoadScene()
    {
        if (EditorApplication.isPlaying)
        {
            Debug.LogError("Not at runtime");
            return;
        }
        EditorSceneManager.OpenScene("Assets/Scenes/MenuScene.unity");
    }
    [MenuItem("FafaStudio/Launch Menu Scene", priority = 1)]
    public static void LoadAndStartScene()
    {
        if (EditorApplication.isPlaying)
        {
            Debug.LogError("Not at runtime");
            return;
        }
        LoadScene();
        EditorApplication.EnterPlaymode();

    }
}
