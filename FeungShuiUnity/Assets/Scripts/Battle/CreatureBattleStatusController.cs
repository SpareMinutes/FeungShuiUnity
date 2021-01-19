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

    private float[] statChanges = {1, 1, 1, 1, 1, 1, 1};

    private Stat statChangedFromStatus;
    private float statChangeAmount;

    void Start() {
        Name.GetComponent<Text>().text = Target.GetName();
        sprite.GetComponent<Image>().sprite = Target.species.battleSprite;
        Target.init();
        if (!Target.playerOwned) {
            Target.currentActiveHealth = Target.getMaxActiveHealth();
            Target.currentCriticalHealth = Target.getMaxCriticalHealth();
            Target.currentMana = Target.getMaxMana();
        }
    }
    
    void Update() {
        //Update active health bar
        float width = Target.currentActiveHealth > 0 ? Mathf.Max(0.25f, 6f * (Target.currentActiveHealth / Target.getMaxActiveHealth())) : 0;
        Stamina.transform.localScale = new Vector3(width, 1, 0);
        //Update critical health bar
        width = Target.currentCriticalHealth > 0 ? Mathf.Max(0.25f, 3f * (Target.currentCriticalHealth / Target.getMaxCriticalHealth())) : 0;
        Critical.transform.localScale = new Vector3(width, 1, 0);

        //Faint
        if (Target.currentActiveHealth <= 0) {
            //remove Target from the Turn manager list
            ES.GetComponent<TurnManager>().removeFromPlay(this);
            gameObject.SetActive(false);

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
        return Physical ? Target.getStat(2)*statChanges[2] : Target.getStat(4) * statChanges[4];
    }

    public float getDefense(bool Physical) {
        float def = Physical ? Target.getStat(3) * statChanges[3] : Target.getStat(5) * statChanges[5];
        if (isDefending) def *= 2;
        return def;
    }

    public float getSpeed() {
        return Target.getStat(6) * statChanges[6];
    }

    public void ApplyDamage (float damageToTake, CreatureBattleStatusController damageTarget) {
        if (damageTarget.GetCreature().currentActiveHealth > damageToTake) {//No damage to crit health, just reduce active health
            damageTarget.GetCreature().currentActiveHealth -= damageToTake;
        } else {                                                            //No active health remaning, damage crit health
            damageToTake -= damageTarget.GetCreature().currentActiveHealth; //How much of the damage wasn't used reducing the active health to 0?
            damageTarget.GetCreature().currentActiveHealth = 0;
            damageTarget.GetCreature().currentCriticalHealth = Mathf.Max(0, damageTarget.GetCreature().currentCriticalHealth - damageToTake);
        }
        damageTarget.GetCreature().currentActiveHealth = Mathf.Min(damageTarget.GetCreature().currentActiveHealth, damageTarget.GetCreature().getMaxActiveHealth());
    }

    #region Stat changes
    public void setDefending(bool defending) {
        isDefending = defending;
    }

    private void changeStat (Stat stat, float amount) {
        changeStat((int)stat-1, amount);
    }

    private void changeStat(int stat, float amount) {
        statChanges[stat] = Mathf.Clamp(statChanges[stat]*amount, 0.5f, 2);
    }

    public void resetStat(Stat stat) {
        resetStat((int)stat-1);
    }

    public void resetStat(int stat) {
        statChanges[stat] = 1;
    }

    public void resetStats() {
        for(int i=0; i<7; i++) {
            resetStat(i);
        }
    }

    public void ChangeStatFromStatus(Stat stat, float amount) {
        statChangedFromStatus = stat;
        statChangeAmount = amount;
        changeStat(statChangedFromStatus, statChangeAmount);
    }

    public void RestoreStatFromStatus() {
        statChangedFromStatus = Stat.None;
        statChangeAmount = 1;
        changeStat(statChangedFromStatus, 1 / statChangeAmount);
    }
    #endregion
}
