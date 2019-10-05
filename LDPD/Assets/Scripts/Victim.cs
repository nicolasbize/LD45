using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Victim : MonoBehaviour
{
    private float timer = 0f;
    private bool activated = false;
    public Sprite asleep;
    public Sprite awakening;
    public Sprite screaming;
    public Sprite shotOnce;
    public Sprite shotTwice;
    public Sprite deadOne;
    public Sprite deadTwo;

    public void Activate() {
        activated = true;
    }


    // Update is called once per frame
    void Update()
    {
        if (activated) {
            timer += Time.deltaTime;

            if (timer > 4) {
                GetComponent<SpriteRenderer>().sprite = awakening;
            }

            if (timer > 6) {
                GetComponent<SpriteRenderer>().sprite = screaming;
            }

            if (timer > 10) {
                GetComponent<SpriteRenderer>().sprite = shotOnce;
            }
            if (timer > 11) {
                GetComponent<SpriteRenderer>().sprite = shotTwice;
            }
            if (timer > 13) {
                GetComponent<SpriteRenderer>().sprite = deadOne;
            }
            if (timer > 15) {
                GetComponent<SpriteRenderer>().sprite = deadTwo;
            }
        }
    }
}
