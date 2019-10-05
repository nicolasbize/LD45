using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    public GameObject characterReference;
    public float padding;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MainCharacter.State state = characterReference.GetComponent<MainCharacter>().GetState();
        float charX = characterReference.transform.position.x;
        float x = transform.position.x;
        if (state == MainCharacter.State.WalkingLeft && charX < x - padding) {
            transform.position = new Vector3(
                characterReference.transform.position.x + padding,
                transform.position.y,
                transform.position.z);
        } else if (state == MainCharacter.State.WalkingRight && charX > x + padding) {
            transform.position = new Vector3(
                characterReference.transform.position.x - padding,
                transform.position.y,
                transform.position.z);
        }
    }

    public void Reset() {
        transform.position = new Vector3(
            characterReference.transform.position.x,
            transform.position.y,
            transform.position.z);
    }
}
