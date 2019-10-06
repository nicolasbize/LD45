using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CodeSubButton : MonoBehaviour, IPointerClickHandler {

    public int increaseValue;

    public void OnPointerClick(PointerEventData eventData) {
        this.transform.parent.GetComponent<CodeButton>().Increase(increaseValue);
    }
}
