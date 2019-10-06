using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    private List<GameObject> inventory = new List<GameObject>();

    public void RemoveFromInventory(GameObject go) {
        GameObject toRemove = null;
        foreach (GameObject obj in inventory) {
            if (obj.name == go.name) {
                toRemove = obj;
            }
        }
        if (toRemove != null) {
            inventory.Remove(toRemove);
            RefreshInventory();
        }
    }

    public void AddToInventory(GameObject go) {
        if (!inventory.Contains(go)) {
            inventory.Add(go);
            RefreshInventory();
        }
    }

    private void RefreshInventory() {
        GameObject canvas = GameObject.Find("InventoryCanvas");
        RawImage[] images = canvas.GetComponentsInChildren<RawImage>();
        for (var i=0; i<inventory.Count; i++) {
            images[i].texture = inventory[i].GetComponent<SpriteRenderer>().sprite.texture;
        }
    }
}
