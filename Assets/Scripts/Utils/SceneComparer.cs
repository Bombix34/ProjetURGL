using System.IO;

public static class SceneComparer
{
    public static bool Compare(string firstScene, string secondScene)
    {
        firstScene = Path.GetFileNameWithoutExtension(firstScene);
        secondScene = Path.GetFileNameWithoutExtension(secondScene);
        return firstScene == secondScene;
    }
}
