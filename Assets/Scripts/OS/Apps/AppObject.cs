using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "App", menuName = "Apps/AppObject", order = 1)]
public class AppObject : ScriptableObject
{
    public string appName;

    public Sprite appIcon;

    public string sceneName;
}
