using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Fade : MonoBehaviour {
    public GameObject sprite, player;

    private void OnTriggerEnter2D(Collider2D collision) {
        if(player.transform.position.z > sprite.transform.position.z) {
            sprite.GetComponent<Tilemap>().color = new Color(1f, 1f, 1f, 0.5f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        sprite.GetComponent<Tilemap>().color = new Color(1f, 1f, 1f, 1f);
    }
}
