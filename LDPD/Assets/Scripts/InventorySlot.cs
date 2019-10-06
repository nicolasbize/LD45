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
            cursorManager.isUsingUIObject = true;
            cursorManager.SetCustomCursor(GetComponent<RawImage>().texture.name);
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        cursorManager.isHoveringUI = true;
        if (HasItem()) {
            cursorManager.SetInventoryCursor();
        }
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

}
