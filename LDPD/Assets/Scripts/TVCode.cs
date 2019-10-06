using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TVCode : MonoBehaviour
{
    public GameObject inventory;
    public GameObject code;
    public GameObject hero;
    private Vector3 prevCameraPosition;

    public void Activate() {
        inventory.SetActive(false);
        code.SetActive(true);
        hero.SetActive(false);
        prevCameraPosition = Camera.main.transform.position;
        Camera.main.transform.position = Vector3.zero;
        GameObject.Find("GameLogic").GetComponent<CursorManager>().ResetCursor();
    }

    public void Deactivate() {
        inventory.SetActive(true);
        code.SetActive(false);
        hero.SetActive(true);
        Camera.main.transform.position = prevCameraPosition;
        GameObject.Find("GameLogic").GetComponent<CursorManager>().ResetCursor();
    }

    public void Success() {
        Deactivate();
    }


}
