using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//Encapsulates Creature for use in battle
public class CreatureBattleStatusController : MonoBehaviour {
    //Info about parent controller   
    public PartyBattleStatusController Parent;
    public int index;
    //Other objects
    public EventSystem ES;
    public GameObject Name, Critical, Stamina, Mana;
    public Image sprite;
    //Status
    public Creature Target;

    [HideInInspector]
    public bool isDefending = false;
    public StatusEffect statusEffect;

    private float[] statChanges = {1, 1, 1, 1, 1, 1, 1};

    private Stat statChangedFromStatus;
    private float statChangeAmount;

    public void SetTarget(Creature TargetIn) {
        Target = TargetIn;
        if(Target == null) {
            gameObject.SetActive(false);
            sprite.enabled = false;
            return;
        } else {
            gameObject.SetActive(true);
            sprite.enabled = true;
        }
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
        //Update mana bar
        width = Target.currentCriticalHealth > 0 ? Mathf.Max(0.25f, 6f * (Target.currentMana / Target.getMaxMana())) : 0;
        Mana.transform.localScale = new Vector3(width, 1, 0);
    }

    public Creature GetCreature() {
        return Target;
    }

    #region Getting stats
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
    #endregion

    #region Damage
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

    public void Faint() {
        //remove Target from the Turn manager list
        ES.GetComponent<TurnManager>().removeFromPlay(this);

        SetTarget(null);
        Parent.ChooseNew(this);
    }

    public void Die() {
        //remove Target from the Turn manager list
        ES.GetComponent<TurnManager>().removeFromPlay(this);

        SetTarget(null);
        Parent.Remove(this);
    }
    #endregion

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
