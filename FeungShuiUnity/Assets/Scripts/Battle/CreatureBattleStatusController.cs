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
        float width = Target.currentActiveHealth>0 ? Mathf.Max(0.25f, 7.5f*(Target.currentActiveHealth/Target.maxActiveHealth)) : 0;
        Stamina.transform.localScale = new Vector3(width, 1, 0);
        width = Target.currentCriticalHealth>0 ? Mathf.Max(0.25f, 4f*(Target.currentCriticalHealth/Target.maxCriticalHealth)) : 0;
        Critical.transform.localScale = new Vector3(width, 1, 0);
        if (Target.currentActiveHealth <= 0){
            //remove Target from the Turn manager list
            ES.GetComponent<TurnManager>().removeFromPlay(Target);
            this.gameObject.SetActive(false);

            //To do: ask for replacement
                //would just reassign the Target
                //ask player to choose from their party 
                //enemy AI would just choose a random non fainted spirit from their party
                //selecton would happen at the end of combat in the menu (todo)
        }
    }
}
