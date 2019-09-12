using UnityEngine;

public class PersistentStats : MonoBehaviour {
    //stores the player postion fore use cross scenes
    public static bool PlayerHasMoved = false;
    public static bool SceneChanged = false;
    public static float PlayerPosX, SceneChangePosX;
    public static float PlayerPosY, SceneChangePosY;
    public static int PlayerRotation = 0;
    
}
