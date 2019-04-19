using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallLife : MonoBehaviour
{
    private float life;
    void Start()
    {
        life = Time.time;
    }


    void Update()
    {
        if (Time.time - life >= 2) {
            Destroy(gameObject);
        }
    }
}
