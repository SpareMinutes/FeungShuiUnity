using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class EventTrigger : MonoBehaviour {
    public UnityEvent onInteract;
    private Creature[] OpposingParty;

    public void StartBattle(){
        //save the players current position and rotation
        GameObject player = GameObject.Find("WalkableCharacter");
        if (player != null) {
            PersistentStats.PlayerPosX = player.transform.position.x;
            PersistentStats.PlayerPosY = player.transform.position.y;
            PersistentStats.PlayerRotation = player.GetComponent<Animator>().GetInteger("angle");
            //Debug.Log(PersistentStats.PlayerRotation);
        }
        //load the battle scene
        SceneManager.LoadSceneAsync("Battle_GUI", LoadSceneMode.Single);
    }

    public void UIMessage(string msg){
        GameObject.Find("InGameUI").GetComponent<MenuAndWorldUI>().ShowMessage(msg);
    }

    public void StartTrainerBattle() {
        OpposingParty = GetComponentInParent<Battle>().Party;
        StartBattle();
        SceneManager.sceneLoaded += LoadOpponent;
    }

    private void LoadOpponent(Scene scene, LoadSceneMode mode) {
        GameObject.Find("Spirit4Status").GetComponent<CreatureBattleStatusController>().Target = OpposingParty[0];
        GameObject.Find("Spirit3Status").GetComponent<CreatureBattleStatusController>().Target = OpposingParty[1];
        SceneManager.sceneLoaded -= LoadOpponent;
    }
}
