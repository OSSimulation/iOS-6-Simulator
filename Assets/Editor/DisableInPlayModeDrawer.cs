using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(DisableInPlayModeAttribute))]
public class DisableInPlayModeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        GUI.enabled = !Application.isPlaying;
        EditorGUI.PropertyField(position, property, label);
        GUI.enabled = true;
    }
}
