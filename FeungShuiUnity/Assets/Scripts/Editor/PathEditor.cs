using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PathAI))]
public class PathEditor : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
    }

    private void OnSceneGUI() {
        PathAI path = (PathAI)target;
        foreach (PathAI.Waypoint waypoint in path.waypoints) {
            if (!waypoint.visible)
                continue;

            float size = HandleUtility.GetHandleSize(waypoint.location) * 0.1f;
            Vector3 snap = Vector3.one * 0.5f;

            EditorGUI.BeginChangeCheck();
            Vector3 newLocation = Handles.FreeMoveHandle(waypoint.location, Quaternion.identity, size, snap, Handles.DotHandleCap);
            Handles.color = Color.clear;
            Vector3 lookAt = new Vector3(waypoint.location.x + Mathf.Cos(waypoint.rotation*Mathf.Deg2Rad)*size*10, waypoint.location.y + Mathf.Sin(waypoint.rotation * Mathf.Deg2Rad)*size*10, 0);
            Vector3 newRotation = Handles.FreeMoveHandle(lookAt, Quaternion.identity, size, snap, Handles.CircleHandleCap);
            Handles.color = Color.white;
            Handles.ArrowHandleCap(0, waypoint.location, Quaternion.Euler(90, 0, 0)*Quaternion.Euler(0, waypoint.rotation+90, 0), size*10, EventType.Repaint);
            if (EditorGUI.EndChangeCheck()) {
                Undo.RecordObject(path, "Change Waypoint");
                waypoint.location = newLocation;
                waypoint.rotation = (int)(Mathf.Atan2(newRotation.y-newLocation.y, newRotation.x-newLocation.x)*Mathf.Rad2Deg);
                path.Update();
            }
        }

    }
}
