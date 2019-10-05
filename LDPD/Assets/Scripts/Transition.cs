using System;
using System.Collections.Generic;
using UnityEngine;

public class Transition : MonoBehaviour
{

    private float alpha = 0;
    private bool isFadingIn = false;
    private bool isFadingOut = false;
    private Action callback = null;

    public void FadeIn(Action callback) {
        isFadingIn = true;
        this.callback = callback;
    }

    public void FadeOut() {
        isFadingOut = true;
    }

    void Update() {
        if (isFadingIn && alpha < 1) {
            alpha += Time.deltaTime;
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, alpha);
            if (alpha >= 1) {
                alpha = 1;
                isFadingIn = false;
                if (callback != null) {
                    callback.Invoke();
                }
            }
        } else if (isFadingOut && alpha > 0) {
            alpha -= Time.deltaTime;
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, alpha);
            if (alpha <= 0) {
                alpha = 0;
                isFadingOut = false;
            }
        }
    }

}
