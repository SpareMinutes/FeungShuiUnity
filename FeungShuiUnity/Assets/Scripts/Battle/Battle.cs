using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Battle : MonoBehaviour{
    public List<Creature> Party;
    private Creature[] PlayerParty, OpposingParty;
    public bool defeated = false, challenged = false;

    public void StartBattle() {
        PlayerParty = GameObject.Find("WalkableCharacter").GetComponent<Battle>().Party.ToArray();
        //load the battle scene
        GameObject.Find("EventSystem").GetComponent<OverworldUI>().OpenNewMenu("BattleScreen");
        challenged = true;
    }

    public void StartWildBattle(Creature[] opposition) {
        PlayerParty = Party.ToArray();
        OpposingParty = opposition;
        StartBattle();
        SceneManager.sceneLoaded += LoadParties;
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
        SceneManager.sceneLoaded -= LoadParties;
    }
}
