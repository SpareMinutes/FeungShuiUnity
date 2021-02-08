using UnityEngine;
using UnityEngine.SceneManagement;

[CreateNodeMenu("Interactions/Actions/Open Shop")]
public class OpenShopNode : InteractionNode {
    [Input(backingValue = ShowBackingValue.Never)] public bool previous;
    [Output] public bool next; 

    private Inventory npcInv;
    private GameObject context;

    public override void Execute(GameObject contextIn) {
        context = contextIn;

        //get the relevant inventories
        npcInv = context.GetComponent<Inventory>();

        //overlay the shop menu
        GameObject.Find("EventSystem").GetComponent<OverworldUI>().SetActiveNode(this);
        GameObject.Find("EventSystem").GetComponent<OverworldUI>().OpenNewMenu("ShopMenu");
        
        SceneManager.sceneLoaded += loadInventories;
    }

    private void loadInventories (Scene scene, LoadSceneMode mode) {
        ShopMenu shopMenu = GameObject.Find("EventSystem").GetComponent<ShopMenu>();
        shopMenu.npcInv = npcInv;
        shopMenu.playerInv = GameObject.Find("WalkableCharacter").GetComponent<Inventory>();
        SceneManager.sceneLoaded -= loadInventories;
    }

    public void Finish() {
        ExecuteNext(GetOutputPort("next"), context);
    }
}
