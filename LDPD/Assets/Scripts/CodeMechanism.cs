using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeMechanism : MonoBehaviour
{
    public string cypher;
    private float timer = 0;
    private bool success = false;

    public void Refresh() {
        string finalCode = "";
        foreach (CodeButton button in GetComponentsInChildren<CodeButton>()) {
            finalCode += button.GetValue();
        }
        if (finalCode == cypher) {
            foreach (CodeButton button in GetComponentsInChildren<CodeButton>()) {
                button.SetGreen();
            }
            success = true;
        }
    }

    void Update() {
        if (success) {
            timer += Time.deltaTime;
            if (timer > 1) {
                GameObject.Find("TVCode").GetComponent<TVCode>().Success();
                InventoryManager inventoryManager = GameObject.Find("Inventory").GetComponent<InventoryManager>();
                inventoryManager.AddToInventory(GameObject.Find("vhs"));
                GameObject.Find("Hero").GetComponent<MainCharacter>().Say(new string[] {
                    "Looks like it unlocked the TV mechanism.",
                    "There's a tape inside the recorder."
                });
                GameObject.Find("SamTV").GetComponent<InteractiveObject>().canInspect = false;
                GameObject.Find("SamTV").GetComponent<InteractiveObject>().canUse = true;
                GameObject.Find("SamTV").GetComponent<InteractiveObject>().requiresItemForUse = true;
                GameObject.Find("SamTV").GetComponent<InteractiveObject>().validItemName = "vhs";
            }
        }
    }
}
