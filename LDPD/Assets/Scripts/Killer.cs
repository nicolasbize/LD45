using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killer : MonoBehaviour
{
    private float timer = 0f;
    private bool activated = false;
    private bool hidden = true;
    public Sprite idle;
    public Sprite aboutToShoot;
    public Sprite shooting;
    private float alpha = 0f;

    public void Activate() {
        activated = true;
    }


    // Update is called once per frame
    void Update() {
        if (activated) {
            timer += Time.deltaTime;
            if (hidden && timer > 2) {
                alpha += Time.deltaTime;
                if (alpha > 1) {
                    alpha = 1;
                    hidden = false;
                    timer = 0;
                }
                GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, alpha);
            } else {
                GameObject.Find("Victim").GetComponent<Victim>().Activate();

                if (timer > 6 && timer < 6.5) {
                    GetComponent<SpriteRenderer>().sprite = aboutToShoot;
                }
                if (timer > 6.5 && timer < 6.7) {
                    GetComponent<SpriteRenderer>().sprite = shooting;
                }
                if (timer > 6.7 && timer < 7.2) {
                    GetComponent<SpriteRenderer>().sprite = aboutToShoot;
                }
                if (timer > 7.2 && timer < 7.4) {
                    GetComponent<SpriteRenderer>().sprite = shooting;
                }
                if (timer > 7.4) {
                    GetComponent<SpriteRenderer>().sprite = idle;
                }
            }

        }
    }
}
