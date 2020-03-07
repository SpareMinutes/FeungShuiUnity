using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class EventTrigger : MonoBehaviour {
    public UnityEvent onInteract;
    private Creature[] OpposingParty;
    private Creature[] PlayerParty;

    private void Update() {
        //Does not work
        //if (!SceneManager.GetActiveScene().Equals(SceneManager.GetSceneByName("Battle_GUI")))
        //    GameObject.Find("EventSystem").GetComponent<EventSystem>().enabled = true;
    }

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
        Time.timeScale = 0;
        GameObject.Find("WalkableCharacter").transform.GetChild(0).gameObject.SetActive(false);
        SceneManager.LoadSceneAsync("Battle_GUI", LoadSceneMode.Additive);
    }

    public void UIMessage(string msg){
        GameObject.Find("InGameUI").GetComponent<MenuAndWorldUI>().ShowMessage(msg);
    }

    public void StartTrainerBattle() {
        OpposingParty = GetComponentInParent<Battle>().Party;
        PlayerParty = GameObject.Find("WalkableCharacter").GetComponent<Battle>().Party;
        StartBattle();
        SceneManager.sceneLoaded += LoadParties;
    }

    private void LoadParties(Scene scene, LoadSceneMode mode) {
        GameObject.Find("Spirit4Status").GetComponent<CreatureBattleStatusController>().Target = OpposingParty[0];
        GameObject.Find("Spirit3Status").GetComponent<CreatureBattleStatusController>().Target = OpposingParty[1];
        GameObject.Find("Spirit2Status").GetComponent<CreatureBattleStatusController>().Target = PlayerParty[0];
        GameObject.Find("Spirit1Status").GetComponent<CreatureBattleStatusController>().Target = PlayerParty[1];
        SceneManager.sceneLoaded -= LoadParties;
    }
}
