using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Battle : MonoBehaviour{
    public List<Creature> Party;
    private List<Creature> PlayerParty, OpposingParty;
    public bool defeated = false, challenged = false;
    
    public void StartWildBattle(Creature[] opposition) {
        PlayerParty = Party;
        OpposingParty = new List<Creature>(opposition);
        //load the battle scene
        GameObject.Find("EventSystem").GetComponent<OverworldUI>().OpenNewMenu("BattleScreen");
        SceneManager.sceneLoaded += LoadParties;
    }

    public void StartTrainerBattle() {
        PlayerParty = GameObject.Find("WalkableCharacter").GetComponent<Battle>().Party;
        OpposingParty = GetComponentInParent<Battle>().Party;
        //load the battle scene
        GameObject.Find("EventSystem").GetComponent<OverworldUI>().OpenNewMenu("BattleScreen");
        challenged = true;
        SceneManager.sceneLoaded += LoadParties;
    }

    private void LoadParties(Scene scene, LoadSceneMode mode) {
        GameObject.Find("EnemyStatus").GetComponent<PartyBattleStatusController>().Begin(OpposingParty);
        GameObject.Find("PlayerStatus").GetComponent<PartyBattleStatusController>().Begin(PlayerParty);
        SceneManager.sceneLoaded -= LoadParties;
    }
}
