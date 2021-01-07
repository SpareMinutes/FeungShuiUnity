using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Battle : MonoBehaviour{
    public List<Creature> Party;
    private Creature[] PlayerParty, OpposingParty;
    public bool defeated = false, challenged = false;

    // Start is called before the first frame update
    void Start(){
        
    }

    // Update is called once per frame
    void Update(){
        
    }

    public void StartBattle() {
        PlayerParty = GameObject.Find("WalkableCharacter").GetComponent<Battle>().Party.ToArray();
        //load the battle scene
        GameObject.Find("EventSystem").GetComponent<OverworldUI>().OpenNewMenu("Battle_GUI");
        challenged = true;
    }

    public void StartTrainerBattle() {
        OpposingParty = GetComponentInParent<Battle>().Party.ToArray();
        PlayerParty = GameObject.Find("WalkableCharacter").GetComponent<Battle>().Party.ToArray();
        StartBattle();
        SceneManager.sceneLoaded += LoadParties;
    }

    private void LoadParties(Scene scene, LoadSceneMode mode) {
        GameObject.Find("Spirit4Status").GetComponent<CreatureBattleStatusController>().Target = Party[0];
        GameObject.Find("Spirit3Status").GetComponent<CreatureBattleStatusController>().Target = Party[1];
        GameObject.Find("Spirit2Status").GetComponent<CreatureBattleStatusController>().Target = PlayerParty[0];
        GameObject.Find("Spirit1Status").GetComponent<CreatureBattleStatusController>().Target = PlayerParty[1];
        GameObject.Find("EventSystem").GetComponent<BattleMenu>().interaction = GetComponentInParent<Interaction>();
        SceneManager.sceneLoaded -= LoadParties;
    }
}
