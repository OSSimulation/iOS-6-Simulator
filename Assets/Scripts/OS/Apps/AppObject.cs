using UnityEngine;

[CreateAssetMenu(fileName = "App", menuName = "Apps/AppObject", order = 1)]
public class AppObject : ScriptableObject
{
    public string appName;

    public Sprite appIcon;

    public string sceneName;
}
