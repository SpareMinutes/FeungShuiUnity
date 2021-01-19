using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Battle : MonoBehaviour{
    public List<Creature> Party;
    private Creature[] PlayerParty, OpposingParty;
    public bool defeated = false, challenged = false;
    
    public void StartWildBattle(Creature[] opposition) {
        PlayerParty = Party.ToArray();
        OpposingParty = opposition;
        //load the battle scene
        GameObject.Find("EventSystem").GetComponent<OverworldUI>().OpenNewMenu("BattleScreen");
        SceneManager.sceneLoaded += LoadParties;
    }

    public void StartTrainerBattle() {
        PlayerParty = GameObject.Find("WalkableCharacter").GetComponent<Battle>().Party.ToArray();
        OpposingParty = GetComponentInParent<Battle>().Party.ToArray();
        //load the battle scene
        GameObject.Find("EventSystem").GetComponent<OverworldUI>().OpenNewMenu("BattleScreen");
        challenged = true;
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
