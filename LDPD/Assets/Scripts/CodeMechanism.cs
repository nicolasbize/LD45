using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeMechanism : MonoBehaviour
{
    public string cypher;

    public void Refresh() {
        string finalCode = "";
        foreach (CodeButton code in GetComponentsInChildren<CodeButton>()) {
            finalCode += code.GetValue();
        }
        if (finalCode == cypher) {
            Debug.Log("Good code!");
        }
    }
}
