using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemMenu : Menu {
    public GameObject ItemList, AmountsList;

    public void UpdateItemList(List<Item> items, Inventory inventory, int index) {
        for (int i = -5; i < 6; i++) {
            string itemName;
            string itemNum;
            if (i + index < 0 || i + index >= items.Count) {
                //this is to account for the 5 above and below the actual item button
                itemName = "";
                itemNum = "";
            } else {
                itemName = items[i + index].name;
                itemNum = inventory.itemDict[items[i + index]].ToString();
            }
            //then set the item text to the item
            ItemList.transform.GetChild(i + 5).GetComponentInChildren<Text>().text = itemName;
            AmountsList.transform.GetChild(i + 5).GetComponentInChildren<Text>().text = itemNum;
        }
    }
}
