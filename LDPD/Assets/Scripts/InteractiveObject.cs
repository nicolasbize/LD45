using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveObject : MonoBehaviour
{

    public bool canEnter;
    public bool canInspect;
    public bool canTake;
    public bool canTalk;
    public bool canUse;
    public bool requiresItemForUse;
    public bool destroyAfterUse;
    public GameObject activedAfterUse;
    public string validItemName;
    public string[] errUseMessages;
    public string[] successUseMessages;
    public string doorName;
    public Sprite spriteAfterUse;
    public GameObject[] givenObjects;
    public GameObject[] takenObjects;
    public Conversation.ChatEntry[] conversation;
    
    private CursorManager cursorManager;

    void Start() {
        cursorManager = GameObject.Find("GameLogic").GetComponent<CursorManager>();
    }

    void OnMouseEnter() {
        cursorManager.SetTarget(this);
        if (cursorManager.isUsingUIObject) {
            if (requiresItemForUse && validItemName == cursorManager.itemName) {
                Debug.Log("Correct items aligned: " + validItemName + " " + cursorManager.itemName);
                cursorManager.SetValidCursorColor();
            } else {
                cursorManager.SetInvalidCursorColor();
            }
        }
    }

    void OnMouseExit() {
        if (!cursorManager.isUsingUIObject) {
            cursorManager.ResetCursor();
        } else {
            if (requiresItemForUse && validItemName == cursorManager.itemName) {
                cursorManager.SetInvalidCursorColor();
            }
        }
    }
    public void Act() {
        if (canTake) {
            GameObject.Find("Hero").GetComponent<MainCharacter>().StartConversation(conversation);
            foreach(GameObject obj in givenObjects) {
                GameObject.Find("Inventory").GetComponent<InventoryManager>().AddToInventory(obj);
            }
            cursorManager.ResetCursor();
            canTake = false;
            if (destroyAfterUse) {
                Destroy(transform.gameObject);
            }
            if (gameObject.name == "ShrededPaper") {
                GameObject.Find("GameLogic").GetComponent<ShreddedPapers>().Activate();
            }
        } else if (canEnter) {
            //if (GameObject.Find("GameLogic").GetComponent<GameManager>().canLeaveLDPD == false) {
            //    GameObject.Find("Hero").GetComponent<MainCharacter>().StartConversation(
            //        new Conversation.ChatEntry[]{
            //        new Conversation.ChatEntry() {
            //            speaker = GameObject.Find("Hero"),
            //            text = new string[] {
            //                "I need to go talk with the Chief before leaving.",
            //            }
            //        }});

            //} else {
                // we need to switch backgrounds
                transform.root.position = new Vector3(0, -20, 0);
                GameObject door = GameObject.Find(doorName);
                door.transform.root.position = new Vector3(0, 0.3f, 0);
                GameObject hero = GameObject.Find("Hero");
                Vector3 p = hero.transform.position;
                hero.transform.position = new Vector3(door.transform.position.x, p.y, p.z);
                hero.GetComponent<MainCharacter>().SetConstraints(door.transform.root);
                GameObject.Find("Main Camera").GetComponent<CameraMovement>().Reset();
            //}
        } else if (canInspect && !cursorManager.isUsingUIObject) {
            GameObject.Find("Hero").GetComponent<MainCharacter>().StartConversation(conversation);
        } else if (canTalk && !cursorManager.isUsingUIObject) {
            GameObject.Find("Hero").GetComponent<MainCharacter>().StartConversation(conversation);
            if (this.name == "Chief") {
                GameObject.Find("GameLogic").GetComponent<GameManager>().canLeaveLDPD = true;
                conversation = new Conversation.ChatEntry[]{
                    new Conversation.ChatEntry() {
                        speaker = this.gameObject,
                        text = new string[] {
                            "McBride, I don't have time to chat. Get some work done!",
                        }
                    }};
            }
        } else if (canUse) {
            if (requiresItemForUse) {
                if (cursorManager.isValidItemUsage) {
                    GameObject.Find("Hero").GetComponent<MainCharacter>().Say(successUseMessages);
                    if (spriteAfterUse != null) {
                        GetComponent<SpriteRenderer>().sprite = spriteAfterUse;
                    }
                    if (activedAfterUse != null) {
                        activedAfterUse.SetActive(true);
                        canUse = false;
                        canInspect = true;
                        Debug.Log("activated");
                    }
                    if (destroyAfterUse) {
                        Destroy(transform.gameObject);
                    }
                    foreach (GameObject obj in takenObjects) {
                        GameObject.Find("Inventory").GetComponent<InventoryManager>().RemoveFromInventory(obj);
                    }
                    foreach (GameObject obj in givenObjects) {
                        GameObject.Find("Inventory").GetComponent<InventoryManager>().AddToInventory(obj);
                    }
                    cursorManager.isUsingUIObject = false;
                    cursorManager.isValidItemUsage = false;
                    cursorManager.ResetCursor();
                } else {
                    GameObject.Find("Hero").GetComponent<MainCharacter>().Say(errUseMessages);
                }
            } else {
                if (gameObject.name == "TVCode") {
                    GetComponent<TVCode>().Activate();
                } else {
                    GameObject.Find("Hero").GetComponent<MainCharacter>().StartConversation(conversation);
                    if (activedAfterUse != null) {
                        activedAfterUse.SetActive(true);
                        Debug.Log("activated");
                    }
                    if (destroyAfterUse) {
                        Destroy(transform.gameObject);
                    }
                }
            }
            cursorManager.ResetCursor();
        }
    }

}
