using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveObject : MonoBehaviour
{

    public bool canEnter;
    public bool canInspect;
    public bool canUse;
    public string doorName;
    public Conversation.ChatEntry[] conversation;
    
    private CursorManager cursorManager;

    void Start() {
        cursorManager = GameObject.Find("GameLogic").GetComponent<CursorManager>();
    }

    void OnMouseEnter() {
        cursorManager.SetTarget(this);
    }

    void OnMouseExit() {
        cursorManager.ResetCursor();
    }

    public void Act() {
        if (canEnter) {
            // we need to switch backgrounds
            transform.root.position = new Vector3(0, -20, 0);
            GameObject door = GameObject.Find(doorName);
            door.transform.root.position = Vector3.zero;
            GameObject hero = GameObject.Find("Hero");
            Vector3 p = hero.transform.position;
            hero.transform.position = new Vector3(door.transform.position.x, p.y, p.z);
            hero.GetComponent<MainCharacter>().SetConstraints(door.transform.root);
            GameObject.Find("Main Camera").GetComponent<CameraMovement>().Reset();
        } else if (canInspect) {
            GameObject.Find("Hero").GetComponent<MainCharacter>().StartConversation(conversation);
        }
    }

}
