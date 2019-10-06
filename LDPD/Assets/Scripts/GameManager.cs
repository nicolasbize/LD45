using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum Scene { TitleScreen, IntroText, IntroMurder, IntroPoliceStation }
    public Scene currentScene;
    private CursorManager cursorManager;
    private GameObject hero;
    private Transition black;
    private GameObject titleScreen;
    private GameObject introText;
    private GameObject apartmentDark;
    private GameObject ldpd;
    private GameObject inventory;

    private float timer = 0;

    public bool canLeaveLDPD = false;

    // Start is called before the first frame update
    void Start()
    {
        black = GameObject.Find("Black").GetComponent<Transition>();
        introText = GameObject.Find("IntroText");
        apartmentDark = GameObject.Find("ApartmentDark");
        titleScreen = GameObject.Find("Title");
        ldpd = GameObject.Find("LDPD");
        inventory = GameObject.Find("Inventory");
        introText.SetActive(false);
        apartmentDark.SetActive(false);
        inventory.SetActive(false);
        ldpd.SetActive(false);
        hero = GameObject.Find("Hero");
        cursorManager = GameObject.Find("GameLogic").GetComponent<CursorManager>();
        LoadScene(currentScene);

        // DELETE THIS
        InventoryManager inventoryManager = GameObject.Find("Inventory").GetComponent<InventoryManager>();
        inventoryManager.AddToInventory(GameObject.Find("armed-slingshot"));
        inventoryManager.AddToInventory(GameObject.Find("claw"));
        //GetComponent<ShreddedPapers>().Activate();
    }

    void Update() {
        if (currentScene == Scene.TitleScreen) {
            if (Input.GetMouseButtonDown(0)) {
                InteractiveObject interactive = cursorManager.GetCurrentTarget();
                if (interactive != null && GameObject.Find("StartGameButton") == interactive.gameObject) {
                    // start game
                    cursorManager.ResetCursor();
                    black.FadeIn(() => {
                        LoadScene(Scene.IntroText);
                    });
                }
            }
        } else if (currentScene == Scene.IntroText) {
            if (Input.GetMouseButton(0)) {
                black.FadeIn(() => {
                    LoadScene(Scene.IntroMurder);
                });
            }
        } else if (currentScene == Scene.IntroMurder) {
            timer += Time.deltaTime;
            if (timer > 2 && timer < 27) {
                if (Camera.main.transform.position.x < 4.19) {
                    Camera.main.transform.Translate(new Vector3(Time.deltaTime, 0, 0));
                } else {
                    GameObject.Find("Killer").GetComponent<Killer>().Activate();
                }
            } else if (timer > 27) {
                black.FadeIn(() => {
                    LoadScene(Scene.IntroPoliceStation);
                });
            }
        }
    }

    private void LoadScene(Scene sceneName) {
        currentScene = sceneName;
        if (currentScene == Scene.TitleScreen) {
            GameObject.Find("Title").transform.position = Vector3.zero;
            hero.SetActive(false);
        } else if (currentScene == Scene.IntroText) {
            titleScreen.SetActive(false);
            introText.SetActive(true);
            black.FadeOut();
        } else if (currentScene == Scene.IntroMurder) {
            introText.SetActive(false);
            apartmentDark.SetActive(true);
            Camera.main.transform.position = new Vector3(-4, 0, -10);
            black.FadeOut();
        } else if (currentScene == Scene.IntroPoliceStation) {
            apartmentDark.SetActive(false);
            hero.SetActive(true);
            ldpd.SetActive(true);
            inventory.SetActive(true);
            ldpd.transform.position = new Vector3(0, 0.3f, 0);
            Camera.main.transform.position = new Vector3(5, 0, -10);
            hero.transform.position = new Vector3(5, 0, -2);
            hero.GetComponent<MainCharacter>().SetConstraints(ldpd.transform.root);
            Conversation conversation = new Conversation();
            hero.GetComponent<MainCharacter>().StartConversation(new Conversation.ChatEntry[]{
                new Conversation.ChatEntry() {
                    speaker = hero,
                    text = new string[] {
                        "Chief Toadler just called me in her office.",
                        "Must be about the raise she keeps telling me about!"
                    }
                }
                });
            black.FadeOut();
        }

    }
}
