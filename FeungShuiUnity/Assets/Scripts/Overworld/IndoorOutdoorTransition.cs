using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IndoorOutdoorTransition : MonoBehaviour {
    
    public string SceneName;
    public bool headingIndoors;
    [Header("Only used for doors going indoors")]
    public direction doorExitDirection;
    public Vector2 newScenePos;

    private void OnTriggerEnter2D(Collider2D other) {
        
        if (other.name == "WalkableCharacter") {
            
            PersistentStats.SceneChanged = true;
            if (headingIndoors) {
                //offset is so it doesnt trigger the trigger when a you go back through the door
                Vector3 offset = Vector3.zero;
                switch (doorExitDirection) {
                    case direction.LEFT : {
                        offset = Vector3.left*12;  break;
                    } case direction.DOWN : {
                        offset = Vector3.down*(16+4);  break;
                    } case direction.RIGHT : {                      //*note these values are based on the Player Capsule collider dimensions
                        offset = Vector3.right*12;  break;
                    } case direction.UP : {
                        offset = Vector3.up*(16+4);  break;
                    }
                }
                //store overworld player location
                PersistentStats.SceneChangePosX = newScenePos.x;
                PersistentStats.SceneChangePosY = newScenePos.y;
                PersistentStats.PlayerOverworldPos = gameObject.transform.position + offset;
            } else {
                //the player is travelling back to the overworld
                PersistentStats.SceneChangePosX = PersistentStats.PlayerOverworldPos.x;
                PersistentStats.SceneChangePosY = PersistentStats.PlayerOverworldPos.y;
                //dont care about pervious pos in the indoor area, since you wil always end up next to the door when entering
            }
            
            //transition scene
            SceneManager.LoadScene(SceneName);
        }
    }

    public enum direction {
        LEFT, RIGHT, UP, DOWN
    }
}
