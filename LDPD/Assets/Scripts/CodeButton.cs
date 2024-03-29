﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CodeButton : MonoBehaviour {

    private int value = 0;
    private bool canMove = true;

    public void Increase(int increaseValue) {
        if (canMove) {
            value += increaseValue;
            if (value > 9) value = 0;
            if (value < 0) value = 9;
            RefreshText();
        }
    }

    private void RefreshText() {
        this.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = value.ToString();
        this.transform.parent.GetComponent<CodeMechanism>().Refresh();
    }

    public int GetValue() {
        return value;
    }

    public void SetGreen() {
        canMove = false;
        this.transform.Find("Text").GetComponent<TextMeshProUGUI>().color = new Color(0, 0.9f, 0);
    }

}
