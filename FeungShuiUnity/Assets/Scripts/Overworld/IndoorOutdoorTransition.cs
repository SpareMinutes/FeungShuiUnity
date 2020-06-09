using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IndoorOutdoorTransition : MonoBehaviour {
    
    public string SceneName;
    public Vector2 newScenePos;

    private void OnTriggerEnter2D(Collider2D other) {
        
        if (other.name == "WalkableCharacter") {
            
            PersistentStats.SceneChanged = true;
            //save the new position that the player will appear in the new scene
            PersistentStats.SceneChangePosX = newScenePos.x;
            PersistentStats.SceneChangePosY = newScenePos.y;
            
            //transition scene
            SceneManager.LoadScene(SceneName);
        }
    }
}
