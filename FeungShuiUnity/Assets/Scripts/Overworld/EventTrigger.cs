using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class EventTrigger : MonoBehaviour {
    public UnityEvent onInteract;

        public void StartBattle(){
        //save the players current position and rotation
        GameObject player = GameObject.Find("WalkableCharacter");
        if (player != null) {
            PersistentStats.PlayerPosX = player.transform.position.x;
            PersistentStats.PlayerPosY = player.transform.position.y;
            //PersistentStats.PlayerRotZ = player.transform.rotation.z;
        }
        //load the battle scene
        SceneManager.LoadScene("Battle_GUI", LoadSceneMode.Single);
    }

    public void UIMessage(string msg){
        GameObject.Find("InGameUI").GetComponent<MenuAndWorldUI>().ShowMessage(msg);
    }

    
}
