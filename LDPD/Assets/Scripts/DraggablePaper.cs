using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggablePaper : MonoBehaviour, IPointerClickHandler {

    public void OnPointerClick(PointerEventData eventData) {
        transform.parent.GetComponent<ShreddedPaperSorter>().ToggleDrag(this.gameObject);
    }
}
