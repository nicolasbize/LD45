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
    private float threshold = 0.01f;
    public float animSpeed = 1;
    private float animCounter = 0;
    public float idleMaxTime = 10;
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

    }

    internal void SetConstraints(Transform root) {
        leftBound = root.Find("LeftBound");
        rightBound = root.Find("RightBound");
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalMovement = 0;
        if (canMove && !cursorManager.isUsingUIObject) {
            horizontalMovement = Input.GetAxis("Horizontal") * speed * Time.deltaTime;    
        }

        if (Input.GetMouseButtonDown(0) && !cursorManager.isHoveringUI) {
            if (chatManager.IsActive()) {
                chatManager.Next();
            } else {
                nextTarget = cursorManager.GetCurrentTarget();
                if (cursorManager.isUsingUIObject) {
                    if (cursorManager.isValidItemUsage) {
                        walkingToTarget = false;
                    } else {
                        cursorManager.isUsingUIObject = false;
                        cursorManager.ResetCursor();
                        Say(new string[] {"That's not gonna work."});
                    }
                } else {
                    if (nextTarget == null) {
                        nextMovePositionX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
                        walkingToTarget = true;
                    } else {
                        walkingToTarget = false;
                    }
                }
            }
        }

        if (Math.Abs(horizontalMovement) > 0) {
            // clear target and give control back as user is using keyboard
            walkingToTarget = false;
            nextTarget = null;
        }

        if (walkingToTarget && Math.Abs(nextMovePositionX - transform.position.x) > threshold) {
            horizontalMovement = speed * Time.deltaTime *
                (nextMovePositionX < transform.position.x ? -1 : 1);
        } else if (nextTarget != null) {
            InteractiveObject actor = nextTarget;
            float distToTarget = Math.Abs(nextTarget.transform.position.x - transform.position.x);
            if (((actor.canInspect || actor.canUse || actor.canEnter) && distToTarget > threshold) || 
                (actor.canTalk && distToTarget > 1.5)) {
                horizontalMovement = speed * Time.deltaTime *
                    (nextTarget.transform.position.x < transform.position.x ? -1 : 1);
            } else {
                // we reached the target, time to act upon it
                nextTarget = null;
                actor.Act();
                if (actor.canInspect || actor.canUse) {
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

        if (Math.Abs(horizontalMovement) > 0 && !chatManager.IsActive()) {
            transform.Translate(horizontalMovement, 0, 0);
            bool isStopped = false;
            if (horizontalMovement < 0 && transform.position.x < leftBound.position.x + leftBound.localScale.x) {
                transform.position = new Vector3(leftBound.position.x + leftBound.localScale.x,
                    transform.position.y, transform.position.z);
                isStopped = true;
            }
            if (horizontalMovement > 0 && transform.localPosition.x > rightBound.localPosition.x) {
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
                if (animCounter > animSpeed || animCounter == 0) {
                    animCounter = 0;
                    currentSpriteIndex = (currentSpriteIndex + 1) % 2;
                    SetSprite(spriteArray[currentSpriteIndex]);
                }
                animCounter += Time.deltaTime;
            }
        }

        
    }

    internal void Say(string[] messages) {
        StartConversation(
            new Conversation.ChatEntry[]{
                new Conversation.ChatEntry() {
                    speaker = gameObject,
                    text = messages
                }});
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
