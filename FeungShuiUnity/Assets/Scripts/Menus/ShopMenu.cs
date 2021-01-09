using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopMenu : ItemMenu {
    private GameObject menu;
    public GameObject SellText, BuyText;
    public int menuType;

    void Start(){
        menu = GameObject.Find("Menu");
        menuType = 0;
    }

    // Update is called once per frame
    void Update() {
        int lastType = menuType;
        if (Input.GetButtonDown("Horizontal")) {
            if (Input.GetAxisRaw("Horizontal") > 0)//right
                menuType = Mathf.Min(1, menuType + 1);
            else//left
                menuType = Mathf.Max(-1, menuType - 1);

            if (lastType != menuType) {
                switch (menuType) {
                    case -1:
                        menu.transform.localPosition = new Vector3(-180, 0, 0);
                        BuyText.SetActive(false);
                        StartCoroutine("MoveRight");
                        break;
                    case 1:
                        menu.transform.localPosition = new Vector3(180, 0, 0);
                        SellText.SetActive(false);
                        StartCoroutine("MoveLeft");
                        break;
                    case 0:
                        BuyText.SetActive(true);
                        SellText.SetActive(true);
                        switch (lastType) {
                            case -1:
                                StartCoroutine("MoveLeft");
                                break;
                            case 1:
                                StartCoroutine("MoveRight");
                                break;
                        }
                        break;
                }
            }
        }
    }

    IEnumerator MoveLeft() {
        for(int i=0; i<60; i++) {
            menu.transform.localPosition = new Vector3(menu.transform.localPosition.x - 2, 0, 0);
            yield return new WaitForSeconds(.001f);
        }
    }

    IEnumerator MoveRight() {
        for (int i = 0; i < 60; i++) {
            menu.transform.localPosition = new Vector3(menu.transform.localPosition.x + 2, 0, 0);
            yield return new WaitForSeconds(.001f);
        }
    }
}
