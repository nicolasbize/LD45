using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacter : MonoBehaviour
{

    public Sprite idle;
    public Sprite[] whistle;
    public Sprite[] walkRight;
    public Sprite[] walkLeft;
    public Sprite faceUp;
    public Sprite reachUp;
    public Sprite crouch;
    public float speed;
    public float animSpeed = 1;
    private float animCounter = 0;
    public float idleMaxTime = 8;
    private float idleCounter = 0;
    private bool canMove = true;
    public enum State { WalkingLeft, WalkingRight, Idle, Whistling };
    private State state = State.Idle;
    private int currentSpriteIndex;

    private InteractiveObject nextTarget;
    private float nextMovePositionX;
    private bool walkingToTarget = false;

    private CursorManager cursorManager;
    private ChatManager chatManager;
    private Transform constraints;
    private Transform leftBound;
    private Transform rightBound;

    void Start() {
        cursorManager = GameObject.Find("GameLogic").GetComponent<CursorManager>();
        chatManager = GameObject.Find("GameLogic").GetComponent<ChatManager>();
        constraints = GameObject.Find("Street").transform;
        SetConstraints(constraints);
    }

    internal void SetConstraints(Transform root) {
        leftBound = root.Find("LeftBound");
        rightBound = root.Find("RightBound");
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalMovement = 0;
        if (canMove) {
            horizontalMovement = Input.GetAxis("Horizontal") * speed * Time.deltaTime;    
        }

        if (Input.GetMouseButtonDown(0)) {
            if (chatManager.IsActive()) {
                chatManager.Next();
            } else {
                nextTarget = cursorManager.GetCurrentTarget();
                if (nextTarget == null) {
                    nextMovePositionX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
                    walkingToTarget = true;
                } else {
                    walkingToTarget = false;
                }
            }
        }

        if (Math.Abs(horizontalMovement) > 0) {
            // clear target and give control back as user is using keyboard
            walkingToTarget = false;
            nextTarget = null;
        }

        if (walkingToTarget && Math.Abs(nextMovePositionX - transform.position.x) > 0.005) {
            horizontalMovement = speed * Time.deltaTime *
                (nextMovePositionX < transform.position.x ? -1 : 1);
        } else if (nextTarget != null) {
            float distToTarget = Math.Abs(nextTarget.transform.position.x - transform.position.x);
            if (distToTarget > 0.005) {
                horizontalMovement = speed * Time.deltaTime *
                    (nextTarget.transform.position.x < transform.position.x ? -1 : 1);
            } else {
                // we reached the target, time to act upon it
                InteractiveObject actor = nextTarget;
                nextTarget = null;
                actor.Act();
                if (actor.canInspect) {
                    SetSprite(faceUp);
                }
            }
        }

        if (horizontalMovement == 0 && state != State.Whistling && state != State.Idle) {
            state = State.Idle;
            idleCounter = 0;
        } else if (horizontalMovement < 0 && state != State.WalkingLeft) {
            state = State.WalkingLeft;
            animCounter = 0;
            currentSpriteIndex = 0;
        } else if (horizontalMovement > 0 && state != State.WalkingRight) {
            state = State.WalkingRight;
            animCounter = 0;
            currentSpriteIndex = 0;
        }

        if (Math.Abs(horizontalMovement) > 0) {
            transform.Translate(horizontalMovement, 0, 0);
            bool isStopped = false;
            if (transform.position.x < leftBound.position.x + leftBound.localScale.x) {
                transform.position = new Vector3(leftBound.position.x + leftBound.localScale.x,
                    transform.position.y, transform.position.z);
                isStopped = true;
            } else if (transform.position.x > rightBound.position.x) {
                transform.position = new Vector3(rightBound.position.x, transform.position.y, transform.position.z);
                isStopped = true;
            }
            if (isStopped) {
                walkingToTarget = false;
            }
        }

        if (chatManager.IsActive()) {
            
        } else {
            // set correct sprite
            if (state == State.Idle) {
                SetSprite(idle);
                idleCounter += Time.deltaTime;
                if (idleCounter > idleMaxTime) {
                    state = State.Whistling;
                    SetSprite(whistle[0]);
                    currentSpriteIndex = 0;
                    animCounter = 0;
                }
            } else {
                Sprite[] spriteArray;
                if (state == State.WalkingLeft) {
                    spriteArray = walkLeft;
                } else if (state == State.WalkingRight) {
                    spriteArray = walkRight;
                } else {
                    spriteArray = whistle;
                }
                animCounter += Time.deltaTime;
                if (animCounter > animSpeed) {
                    animCounter = 0;
                    currentSpriteIndex = (currentSpriteIndex + 1) % 2;
                    SetSprite(spriteArray[currentSpriteIndex]);
                }
            }
        }

        
    }

    internal void StartConversation(Conversation.ChatEntry[] conversation) {
        chatManager = GameObject.Find("GameLogic").GetComponent<ChatManager>();
        chatManager.StartChat(conversation);
    }

    private void SetSprite(Sprite sprite) {
        GetComponent<SpriteRenderer>().sprite = sprite;
    }

    private Sprite GetSprite() {
        return GetComponent<SpriteRenderer>().sprite;
    }

    public State GetState() {
        return state;
    }
}
