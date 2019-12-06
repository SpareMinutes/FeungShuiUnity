using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//Encapsulates Creature for use in battle
public class CreatureBattleStatusController : MonoBehaviour {
    public Creature Target;
    public GameObject Name, Critical, Stamina, Mana;
    public EventSystem ES;
    public Image sprite;

    private float currentAttack;
    private float currentDefense;
    private float currentIntelligence;
    private float currentResistance;
    private float currentSpeed;

    private float preDefenseMoveDefense;
    private float preDefenseMoveResistance;

    // Start is called before the first frame update
    void Start() {
        Target.Init();
        Name.GetComponent<Text>().text = Target.displayName;

        /*
        //pick a random nature
        this.personality = Natures2.personalityKeys[Random.Range(0,Natures2.personalityKeys.Count)];
        this.statModifiers = Natures2.personalityDict[this.personality];

        //this is where the stats will be generated based off (real thing)

        //for testing purposes i'll add moves manually here
        foreach (string name in moveNames) {
            Moves.Add(MovesTable.Master[name]);
        }
        */
        currentAttack = Target.getAttack();
        currentDefense = Target.getDefense();
        currentIntelligence = Target.getIntelligence();
        currentResistance = Target.getResistance();
        currentSpeed = Target.getSpeed();
        preDefenseMoveDefense = currentDefense;
        preDefenseMoveResistance = currentResistance;
        sprite.GetComponent<Image>().sprite = Resources.Load<Sprite>("Spirits/" + Target.speciesName);
    }

    // Update is called once per frame
    void Update(){
        float width = Target.currentActiveHealth>0 ? Mathf.Max(0.25f, 6f*(Target.currentActiveHealth/Target.maxActiveHealth)) : 0;
        if (Stamina.transform.localScale.x != width) {
            Stamina.transform.localScale = new Vector3(width, 1, 0);
            width = Target.currentCriticalHealth > 0 ? Mathf.Max(0.25f, 3f * (Target.currentCriticalHealth / Target.maxCriticalHealth)) : 0;
            Critical.transform.localScale = new Vector3(width, 1, 0);
        }
        if (Target.currentActiveHealth <= 0){
            //remove Target from the Turn manager list
            ES.GetComponent<TurnManager>().removeFromPlay(this);
            this.gameObject.SetActive(false);

            //To do: ask for replacement
            //would just reassign the Target
            //ask player to choose from their party 
            //enemy AI would just choose a random non fainted spirit from their party
            //selecton would happen at the end of combat in the menu (todo)
        }
    }

    public Creature GetCreature() {
        return Target;
    }

    public float getAttack(bool Physical) {
        if (Physical) {
            return this.currentAttack;
        } else {
            return this.currentIntelligence;
        }
    }

    public float getDefense(bool Physical) {
        if (Physical) {
            return this.currentDefense;
        } else {
            return this.currentResistance;
        }
    }

    public float getSpeed() {
        return currentSpeed;
    }

    public void doDefenseMove() {
        //for now just increase defense and special defense by a bunch (very techincal i know)
        //these variables are if you buff your defenses other than this move (eg harden in pokemon)
        preDefenseMoveDefense = currentDefense;
        preDefenseMoveResistance = currentResistance;

        //multiplies the defenses by 2 (up for change)
        this.currentDefense *= 2;
        this.currentResistance *= 2;
    }

    public void relieveDefenseMove() {
        currentDefense = preDefenseMoveDefense;
        currentResistance = preDefenseMoveResistance;

    }
}
