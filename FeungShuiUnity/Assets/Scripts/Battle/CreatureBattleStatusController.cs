using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreatureBattleStatusController : MonoBehaviour {
    public Creature Target;
    public GameObject Name, Critical, Stamina, Mana;

    // Start is called before the first frame update
    void Start(){
        Name.GetComponent<Text>().text = Target.displayName;
    }

    // Update is called once per frame
    void Update(){
        Stamina.transform.localScale = new Vector3(7.5f * (Target.currentActiveHealth / Target.maxActiveHealth), 1, 0);
        Critical.transform.localScale = new Vector3(4f * (Target.currentCriticalHealth / Target.maxCriticalHealth), 1, 0);
        if (Target.currentActiveHealth <= 0){
            this.gameObject.SetActive(false);
            //To do: ask for replacement
        }
    }
}
