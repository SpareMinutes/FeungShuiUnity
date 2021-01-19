using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(PathAI.Waypoint))]
public class WaypointDrawer : PropertyDrawer {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        EditorGUI.BeginProperty(position, label, property);
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        Rect locRect = new Rect(position.x, position.y, 200, position.height/2);
        Rect rotRect = new Rect(position.x, position.y + position.height / 2 + 2, 100, position.height / 2);
        Rect durRect = new Rect(position.x + 100, position.y + position.height / 2 + 2, 100, position.height / 2);
        Rect visRect = new Rect(position.x - 75, position.y + position.height / 2 + 2, 75, position.height / 2);


        EditorGUI.PropertyField(locRect, property.FindPropertyRelative("location"), GUIContent.none);
        EditorGUIUtility.labelWidth = 60;
        label.text = "Rotation";
        EditorGUI.PropertyField(rotRect, property.FindPropertyRelative("rotation"), label);
        label.text = "Duration";
        EditorGUI.PropertyField(durRect, property.FindPropertyRelative("duration"), label);
        label.text = "Visible";
        EditorGUI.PropertyField(visRect, property.FindPropertyRelative("visible"), label);

        EditorGUI.indentLevel = indent;
        EditorGUI.EndProperty();
        EditorGUIUtility.labelWidth = 0;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        return base.GetPropertyHeight(property, label) * 2;
    }
}
