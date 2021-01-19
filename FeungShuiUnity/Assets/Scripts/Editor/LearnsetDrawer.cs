/* using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Species.Learnset))]
public class LearnsetDrawer : PropertyDrawer {
    private bool LevelupFolded;
    private bool[] LevelGroupFolded = new bool[10];

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        EditorGUI.BeginProperty(position, label, property);
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 1;
        LevelupFolded = EditorGUILayout.Foldout(LevelupFolded, "Level Up Moves");
        if (LevelupFolded) {
            EditorGUI.indentLevel = 2;
            for (int i = 0; i<10; i++) {
                LevelGroupFolded[i] = EditorGUILayout.Foldout(LevelGroupFolded[i], "Levels " + i + "1 to " + (i+1) + "0");
                if (LevelGroupFolded[i]) {
                    EditorGUI.indentLevel = 3;
                    for (int j = 0; j < 10; j++) {
                        int index = i * 10 + j;
                        //if(property.FindPropertyRelative("LevelupMoves").arraySize==0)
                            //property.FindPropertyRelative("LevelupMoves")
                        Debug.Log(index + " " + property.FindPropertyRelative("LevelupMoves").arraySize);
                        EditorGUILayout.PropertyField(property.FindPropertyRelative("LevelupMoves").GetArrayElementAtIndex(index), label);
                    }
                }
            }
        }

        EditorGUI.indentLevel = indent;
        EditorGUI.EndProperty();
        EditorGUIUtility.labelWidth = 0;
    }

    //public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
    //    return base.GetPropertyHeight(property, label) * 2;
    //}
} */
