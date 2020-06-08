using UnityEngine;

public class PersistentStats : MonoBehaviour {
    //stores the player postion for use across scenes
    public static bool PlayerHasMoved = false;
    public static bool SceneChanged = false;
    //these are the coords that the player will appear at when the a new scene is loaded
    public static float PlayerPosX, SceneChangePosX;
    public static float PlayerPosY, SceneChangePosY;
    //this is the saved coords of the player when going from outdoors to indoors
    public static Vector3 PlayerOverworldPos;
    public static string outdoorSceneName;
    
    public static int PlayerRotation = 0;
    
}
