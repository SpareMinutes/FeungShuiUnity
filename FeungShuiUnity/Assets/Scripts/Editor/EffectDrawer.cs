using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Effect))]
public class EffectDrawer : PropertyDrawer {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        EditorGUI.BeginProperty(position, label, property);
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        Rect typeRect = new Rect(35, position.y + 20, Screen.width-40, 20);

        label.text = "Effect Type";
        EditorGUI.PropertyField(typeRect, property.FindPropertyRelative("effectType"), label);

        int type = property.FindPropertyRelative("effectType").intValue;
        if (type == 3) {
            Rect statusRect = new Rect(35, position.y + 40, Screen.width - 40, 20);
            label.text = "Status Effect";
            EditorGUI.PropertyField(statusRect, property.FindPropertyRelative("statusEffect"), label);

            Rect chanceRect = new Rect(35, position.y + 60, Screen.width - 40, 20);
            label.text = "Chance";
            EditorGUI.PropertyField(chanceRect, property.FindPropertyRelative("chance"), label);
        }else if(type==4 || type == 5) {
            Rect statRect = new Rect(35, position.y + 40, Screen.width - 40, 20);
            label.text = "Stat";
            EditorGUI.PropertyField(statRect, property.FindPropertyRelative("stat"), label);

            Rect powerRect = new Rect(35, position.y + 60, Screen.width - 40, 20);
            label.text = "Power";
            EditorGUI.PropertyField(powerRect, property.FindPropertyRelative("power"), label);

            Rect chanceRect = new Rect(35, position.y + 80, Screen.width - 40, 20);
            label.text = "Chance";
            EditorGUI.PropertyField(chanceRect, property.FindPropertyRelative("chance"), label);
        } else {
            Rect powerRect = new Rect(35, position.y + 40, Screen.width - 40, 20);
            label.text = "Power";
            EditorGUI.PropertyField(powerRect, property.FindPropertyRelative("power"), label);

            if(type == 2) {
                Rect currentRect = new Rect(35, position.y + 60, Screen.width - 40, 20);
                label.text = "Use Current Health";
                EditorGUI.PropertyField(currentRect, property.FindPropertyRelative("useCurrentHealth"), label);
            }
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        switch (property.FindPropertyRelative("effectType").intValue) {
            case 0:
                return 60;
            case 1:
                return 60;
            case 2:
                return 80;
            case 3:
                return 80;
            case 4:
                return 100;
            case 5:
                return 100;
            default:
                return 0;
        }
    }
}
