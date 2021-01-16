using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class EncounterWild : MonoBehaviour{
    [SerializeField]
    private Tilemap Background;
    private float Counter;
    [SerializeField]
    private Battle Player;

    public string[] TileNames;
    public int[] AltRates;

    public void Start(){
        Counter = 0;
    }

    public void FixedUpdate(){
        if(GetComponent<Animator>().GetBool("isWalking"))
            Counter += Time.fixedDeltaTime;
        if (Counter >= 1){
            string type = Background.GetTile(new Vector3Int((int)(transform.position.x / 16), (int)(transform.position.y / 16), 0)).name;
            float rate = 0.0f;
            if (type.Equals("Grass"))
                rate = 0.2f;
            if (type.Equals("Path"))
                rate = 0.1f;
            float roll = Random.Range(0f, 1f);
            Debug.Log("Rolled for encounter on " + type);
            if (roll < rate)
                Player.StartBattle();
            Counter -= 1;
        }
    }
}
