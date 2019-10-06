using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {

    private CursorManager cursorManager;

    void Start() {
        cursorManager = GameObject.Find("GameLogic").GetComponent<CursorManager>();
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (HasItem()) {
            if (cursorManager.isUsingUIObject) {
                if (isValidItemMatch(GetComponent<RawImage>().texture.name, cursorManager.itemName)) {
                    string str = PerformTrade(GetComponent<RawImage>().texture.name, cursorManager.itemName);
                    GameObject.Find("Hero").GetComponent<MainCharacter>().Say(new string[] {
                        str
                    });
                    cursorManager.isUsingUIObject = false;
                    cursorManager.ResetCursor();
                } else {
                    GameObject.Find("Hero").GetComponent<MainCharacter>().Say(new string[] {
                        "That's a stupid idea, how is this getting through my head?"
                    });
                }
            } else {
                cursorManager.isUsingUIObject = true;
                Debug.Log("Using cursor " + GetComponent<RawImage>().texture.name);
                cursorManager.SetCustomCursor(GetComponent<RawImage>().texture.name);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        cursorManager.isHoveringUI = true;
        if (HasItem()) {
            if (cursorManager.isUsingUIObject) {
                if (isValidItemMatch(GetComponent<RawImage>().texture.name, cursorManager.itemName)) {
                    cursorManager.SetValidCursorColor();
                } else {
                    cursorManager.SetInvalidCursorColor();
                }
            } else {
                cursorManager.SetInventoryCursor();
            }
        }
    }

    private bool isValidItemMatch(string item1, string item2) {
        return ((item1 == "ball" && item2 == "slingshot") ||
            (item1 == "slingshot" && item2 == "ball") ||
            (item1 == "magnet" && item2 == "shoelace") ||
            (item1 == "shoelace" && item2 == "magnet") ||
            (item1 == "envelope" && item2 == "vhs") ||
            (item1 == "vhs" && item2 == "envelope") ||
            (item1 == "envelope" && item2 == "letter") ||
            (item1 == "letter" && item2 == "envelope") ||
            (item1 == "partial-proof" && item2 == "letter") ||
            (item1 == "letter" && item2 == "partial-proof") ||
            (item1 == "partial-proof" && item2 == "vhs") ||
            (item1 == "vhs" && item2 == "partial-proof"));
    }

    public void OnPointerExit(PointerEventData eventData) {
        cursorManager.isHoveringUI = false;
        if (!cursorManager.isUsingUIObject) {
            cursorManager.ResetCursor();
        }
    }

    private bool HasItem() {
        return GetComponent<RawImage>().texture.name != "empty";
    }

    private string PerformTrade(string item1, string item2) {
        InventoryManager inventory = GameObject.Find("Inventory").GetComponent<InventoryManager>();
        inventory.RemoveByName(item1);
        inventory.RemoveByName(item2);
        if (((item1 == "ball" && item2 == "slingshot") ||
            (item1 == "slingshot" && item2 == "ball"))) {
            inventory.AddToInventory(GameObject.Find("armed-slingshot"));
            return "Bring it on, Goliath!";
        }
        if (((item1 == "magnet" && item2 == "shoelace") ||
            (item1 == "shoelace" && item2 == "magnet"))) {
            inventory.AddToInventory(GameObject.Find("claw"));
            return "Well that will make a cute little magnetized claw!";
        }
        if (((item1 == "envelope" && item2 == "vhs") ||
            (item1 == "vhs" && item2 == "envelope"))) {
            inventory.AddToInventory(GameObject.Find("partial-proof"));
            return "This is nice but they'll need some lead to catch them as well.";
        }
        if (((item1 == "envelope" && item2 == "letter") ||
            (item1 == "letter" && item2 == "envelope"))) {
            inventory.AddToInventory(GameObject.Find("partial-proof"));
            return "This is nice but they'll need some more proof before following the lead.";
        }
        if (((item1 == "partial-proof" && item2 == "letter") ||
            (item1 == "letter" && item2 == "partial-proof"))) {
            inventory.AddToInventory(GameObject.Find("full-proof"));
            return "Booya! Get ready to rot in hell guys!";
        }
        if (((item1 == "partial-proof" && item2 == "vhs") ||
            (item1 == "vhs" && item2 == "partial-proof"))) {
            inventory.AddToInventory(GameObject.Find("full-proof"));
            return "Booya! Get ready to rot in hell guys!";
        }
        return "This will work!";
    }

}
