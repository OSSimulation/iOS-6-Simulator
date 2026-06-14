using UnityEngine;

[CreateAssetMenu(fileName = "App", menuName = "Apps/AppObject", order = 1)]
public class AppObject : ScriptableObject
{
    // Update app information to include, unique identifiers and other params

    // Name shown on the Home Screen
    [SerializeField] private string appName;
    public string AppName => appName;

    // The way the iOS 6 Simulator identifies your app
    // Example: com.os6.springboard
    [SerializeField] private string appIdentifier;
    public string AppIdentifier => appIdentifier;

    // The name of the scene that contains your app
    [SerializeField] private string sceneName;
    public string SceneName => sceneName;

    // App Icon to show on the Home Screen
    [SerializeField] private Sprite appIcon;
    public Sprite AppIcon => appIcon;
}
