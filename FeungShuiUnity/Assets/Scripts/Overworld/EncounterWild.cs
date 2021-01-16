using UnityEngine;
using UnityEngine.Tilemaps;


public class EncounterWild : MonoBehaviour{
    [SerializeField]
    private Tilemap Background;
    private float Counter;
    [SerializeField]
    private Battle Player;

    public void Start(){
        Counter = 0;
    }

    public void FixedUpdate(){
        if(GetComponent<Animator>().GetBool("isWalking"))
            Counter += Time.fixedDeltaTime;
        if (Counter >= 1){
            string type = Background.GetTile(new Vector3Int((int)(transform.position.x / 16), (int)(transform.position.y / 16), 0)).name;
            
            foreach (Collider2D contacts in Physics2D.OverlapPointAll(transform.position)) {
                EncounterZone zone = contacts.GetComponent<EncounterZone>();
                if (zone != null) {
                    Creature[] opposition = zone.RollEncounter(type);
                    GetComponent<Battle>().StartWildBattle(opposition);
                    break;
                }
            }
            Debug.Log("Rolled for encounter on " + type);
            Counter -= 1;
        }
    }
}
