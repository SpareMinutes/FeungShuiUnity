using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//Encapsulates Creature for use in battle
public class CreatureBattleStatusController : MonoBehaviour {
    public Creature Target;
    public GameObject Name, Critical, Stamina, Mana;
    public EventSystem ES;
    public Image sprite;
    
    [HideInInspector]
    public bool isDefending = false;
    public StatusEffect statusEffect;
    
    private float currentAttack;
    private float currentDefense;
    private float currentIntelligence;
    private float currentResistance;
    private float currentSpeed;

    private float preDefenseMoveDefense;
    private float preDefenseMoveResistance;

    private Stat statChangedFromStatus;
    private float statChangeAmount;

    // Start is called before the first frame update
    void Start() {
        Target.UpdateStats();
        Name.GetComponent<Text>().text = Target.name;

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

        isDefending = true;
    }

    public void relieveDefenseMove() {
        currentDefense = preDefenseMoveDefense;
        currentResistance = preDefenseMoveResistance;

    }

    public void ApplyDamage (float damageToTake, CreatureBattleStatusController damageTarget) {
        if(damageTarget.GetCreature().currentActiveHealth > damageToTake){
            damageTarget.GetCreature().currentActiveHealth -= damageToTake;
        } else {
            damageToTake -= damageTarget.GetCreature().currentActiveHealth;
            damageTarget.GetCreature().currentActiveHealth = 0;
            damageTarget.GetCreature().currentCriticalHealth = Mathf.Max(0, damageTarget.GetCreature().currentCriticalHealth - damageToTake);
        }
        damageTarget.GetCreature().currentActiveHealth = Mathf.Min(damageTarget.GetCreature().currentActiveHealth, damageTarget.GetCreature().maxActiveHealth);
    }

    public void ChangeStatFromStatus (Stat stat, float amount) {
        statChangedFromStatus = stat;
        statChangeAmount = amount;
        changeStat(statChangedFromStatus, statChangeAmount);
    }

    public void RestoreStatFromStatus () {
        statChangedFromStatus = Stat.None;
        statChangeAmount = 1;
        changeStat(statChangedFromStatus, 1/statChangeAmount);
    }

    private void changeStat (Stat stat, float amount) {
        //just used for when status effects change the stats while they are affected by the status
        switch (statChangedFromStatus) {
            case Stat.Attack : {
                //change attack
                currentAttack *= amount;
                break;
            }case Stat.Defense : {
                //change attack
                currentDefense *= amount;
                break;
            } case Stat.Intelligence : {
                //change attack
                currentIntelligence *= amount;
                break;
            } case Stat.Resistance : {
                //change attack
                currentResistance *= amount;
                break;
            } case Stat.Speed : {
                //change attack
                currentSpeed *= amount;
                break;
            }
        }
    }
}
