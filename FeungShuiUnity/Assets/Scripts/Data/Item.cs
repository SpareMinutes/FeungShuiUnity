using System.Collections;
using System.Collections.Generic;
//using UnityEngine;

public class Item {

    public BagTabs tab; //will be what tab the item should go in (made manually in the editor)
    public int amount = 0; //will keep track of how many of this item hte plaer has in their inventory
    public string displayName;

    //need to have a similar thing to the move to determine what their effect is
    public Item (string name, BagTabs bagTab) {
        displayName = name;
        tab = bagTab;
    }
    
}
