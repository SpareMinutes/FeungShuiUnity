using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basic_game_handler : MonoBehaviour
{
    private GameObject OverallHP;
   
    [SerializeField] private OverallHP healthbar;
    
    private void Start()
    {
        healthbar.SetSize(.4f);
    }

    // Update is called once per frame

}
