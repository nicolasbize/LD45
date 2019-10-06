using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShreddedPaperSorter : MonoBehaviour
{

    private GameObject draggedObject = null;
    private bool isDone = false;
    private float timer = 0;

    public void ToggleDrag(GameObject go) {
        if (draggedObject == null) {
            draggedObject = go;
        } else {
            draggedObject = null;
            if (IsOrderValid()) {
                ProperlySortAndFinish();
                isDone = true;
            }
        }
    }

    void Update() {
        if (isDone == false) {
            GameObject.Find("GameLogic").GetComponent<CursorManager>().SetArrowCursor();
            if (draggedObject) {
                draggedObject.transform.position =
                    new Vector3(Input.mousePosition.x,
                    draggedObject.transform.position.y,
                    draggedObject.transform.position.z); ;
            }
        } else {
            timer += Time.deltaTime;
            if (timer > 5) {
                GameObject.Find("GameLogic").GetComponent<ShreddedPapers>().Success();
            }
        }

    }

    public bool IsOrderValid() {
        List<float> xPositions = new List<float>();
        foreach (DraggablePaper paper in GetComponentsInChildren<DraggablePaper>()) {
            xPositions.Add(paper.GetComponent<RectTransform>().localPosition.x);
        }
        for (int i=0; i<xPositions.Count - 1; i++) {
            if (xPositions[i] >= xPositions[i+1]) {
                return false;
            }
        }
        return true;
    }

    public void ProperlySortAndFinish() {
        DraggablePaper[] papers = GetComponentsInChildren<DraggablePaper>();
        for (int i=0; i<papers.Length; i++) {
            papers[i].GetComponent<RectTransform>().localPosition = new Vector3(24 * i-120, 00, 0);
        }
    }


}
