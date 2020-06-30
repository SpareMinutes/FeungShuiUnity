using UnityEngine;
using UnityEngine.SceneManagement;

public class Battle : MonoBehaviour{
    public Creature[] Party;
    private Creature[] PlayerParty;

    // Start is called before the first frame update
    void Start(){
        
    }

    // Update is called once per frame
    void Update(){
        
    }

    public void StartBattle() {
        PlayerParty = GameObject.Find("WalkableCharacter").GetComponent<Battle>().Party;
        //load the battle scene
        Time.timeScale = 0;
        GameObject.Find("WalkableCharacter").transform.GetChild(0).gameObject.SetActive(false);
        SceneManager.LoadSceneAsync("Battle_GUI", LoadSceneMode.Additive);
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
