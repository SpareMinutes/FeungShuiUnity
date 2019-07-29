using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CreatureBattleStatusController : MonoBehaviour {
    public Creature Target;
    public GameObject Name, Critical, Stamina, Mana;
    public EventSystem ES;

    // Start is called before the first frame update
    void Start(){
        Name.GetComponent<Text>().text = Target.displayName;
    }

    // Update is called once per frame
    void Update(){
        Stamina.transform.localScale = new Vector3(7.5f * (Target.currentActiveHealth / Target.maxActiveHealth), 1, 0);
        Critical.transform.localScale = new Vector3(4f * (Target.currentCriticalHealth / Target.maxCriticalHealth), 1, 0);
        if (Target.currentActiveHealth <= 0){
            //remove Target from the Turn manager list
            ES.GetComponent<TurnManager>().removeFromPlay(Target);

            //after its removed from play set the dead cover up for that spirit
            for (int i=0; i < 4; i++) {
                GameObject spirit = ES.GetComponent<BattleMenu>().spiritStatuses[i];
                
                if (spirit.GetComponent<CreatureBattleStatusController>().Target == this.Target) {
                    GameObject.Find("Spirit" + i + "Cover(Dead)").GetComponent<Image>().enabled = true;
                }
            }
            this.gameObject.SetActive(false);

            //To do: ask for replacement
                //would just reassign the Target
                //ask player to choose from their party 
                //enemy AI would just choose a random non fainted spirit from their party
                //selecton would happen at the end of combat in the menu (todo)
        }
    }
}
