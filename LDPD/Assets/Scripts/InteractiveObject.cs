using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveObject : MonoBehaviour
{

    public bool canEnter;
    public string doorName;
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
            Vector3 p = GameObject.Find("Hero").transform.position;
            GameObject.Find("Hero").transform.position = new Vector3(door.transform.position.x, p.y, p.z);
            GameObject.Find("Main Camera").GetComponent<CameraMovement>().Reset();
        }
    }

}
