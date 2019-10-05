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
    private InteractiveObject currentTarget;

    public enum CursorType { Move, Enter, Chat, Use, View };


    // Start is called before the first frame update
    void Start() {
        ResetCursor();
    }

    internal void SetTarget(InteractiveObject interactiveObject) {
        currentTarget = interactiveObject;
        if (interactiveObject.canEnter) {
            Cursor.SetCursor(enterCursor, Vector2.zero, CursorMode.ForceSoftware);
        } else if (interactiveObject.canInspect) {
            Cursor.SetCursor(viewCursor, Vector2.zero, CursorMode.ForceSoftware);
        }
    }

    public InteractiveObject GetCurrentTarget() {
        return currentTarget;
    }

    public void ResetCursor() {
        Cursor.SetCursor(moveCursor, Vector2.zero, CursorMode.ForceSoftware);
        currentTarget = null;
    }
}
