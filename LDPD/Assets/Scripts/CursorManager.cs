using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour {

    public Texture2D moveCursor;
    public Texture2D enterCursor;
    public Texture2D chatCursor;
    public Texture2D useCursor;
    public Texture2D viewCursor;
    public Texture2D arrowCursor;
    private InteractiveObject currentTarget;
    public bool isHoveringUI;
    public bool isUsingUIObject;
    public string itemName;
    public Texture2D currentInventoryTex;
    public bool isValidItemUsage = false;

    internal void SetCustomCursor(string name) {
        itemName = name;
        SetInvalidCursorColor();
    }

    public enum CursorType { Move, Enter, Chat, Use, View };


    // Start is called before the first frame update
    void Start() {
        ResetCursor();
    }

    internal void SetValidCursorColor() {
        currentInventoryTex = Resources.Load<Texture2D>("cursors/" + itemName);
        Cursor.SetCursor(currentInventoryTex, Vector2.zero, CursorMode.ForceSoftware);
        isValidItemUsage = true;
    }

    internal void SetInvalidCursorColor() {
        currentInventoryTex = Resources.Load<Texture2D>("cursors/" + itemName + "-disabled");
        Cursor.SetCursor(currentInventoryTex, Vector2.zero, CursorMode.ForceSoftware);
        isValidItemUsage = false;
    }

    internal void SetTarget(InteractiveObject interactiveObject) {
        currentTarget = interactiveObject;
        if (!isUsingUIObject) {
            if (interactiveObject.canEnter) {
                Cursor.SetCursor(enterCursor, Vector2.zero, CursorMode.ForceSoftware);
            } else if (interactiveObject.canInspect) {
                Cursor.SetCursor(viewCursor, Vector2.zero, CursorMode.ForceSoftware);
            } else if (interactiveObject.canTake || interactiveObject.canUse) {
                Cursor.SetCursor(useCursor, Vector2.zero, CursorMode.ForceSoftware);
            } else if (interactiveObject.canTalk) {
                Cursor.SetCursor(chatCursor, Vector2.zero, CursorMode.ForceSoftware);
            }
        }
    }

    internal void SetInventoryCursor() {
        Cursor.SetCursor(useCursor, Vector2.zero, CursorMode.ForceSoftware);
        currentTarget = null;
    }

    public InteractiveObject GetCurrentTarget() {
        return currentTarget;
    }

    public void ResetCursor() {
        Cursor.SetCursor(moveCursor, Vector2.zero, CursorMode.ForceSoftware);
        currentTarget = null;
    }
    public void SetArrowCursor() {
        Cursor.SetCursor(arrowCursor, Vector2.zero, CursorMode.ForceSoftware);
        currentTarget = null;
    }
}
