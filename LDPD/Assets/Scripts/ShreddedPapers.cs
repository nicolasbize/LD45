using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShreddedPapers : MonoBehaviour
{
    public GameObject inventory;
    public GameObject hero;
    public GameObject shreddedPaperUi;
    private Vector3 prevCameraPosition;

    public void Activate() {
        inventory.SetActive(false);
        hero.SetActive(false);
        shreddedPaperUi.SetActive(true);
        prevCameraPosition = Camera.main.transform.position;
        Camera.main.transform.position = Vector3.zero;
        GameObject.Find("GameLogic").GetComponent<CursorManager>().SetArrowCursor();
    }

    public void Success() {
        inventory.SetActive(true);
        hero.SetActive(true);
        shreddedPaperUi.SetActive(false);
        Camera.main.transform.position = prevCameraPosition;
        GameObject.Find("GameLogic").GetComponent<CursorManager>().ResetCursor();
        InventoryManager inventoryManager = GameObject.Find("Inventory").GetComponent<InventoryManager>();
        inventoryManager.AddToInventory(GameObject.Find("letter"));
        GameObject.Find("Hero").GetComponent<MainCharacter>().Say(new string[] {
                    "It looks like there's an opportunity to catch them.",
                    "I need to send all of this info to the FBI."
                });
    }
}
