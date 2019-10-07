using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum Scene { TitleScreen, IntroText, IntroMurder, IntroPoliceStation, Ending, FinalEnd }
    public Scene currentScene;
    public AudioClip introMusic;
    public AudioClip mainMusic;
    public AudioClip shootFx;
    private CursorManager cursorManager;
    private GameObject hero;
    private Transition black;
    private GameObject titleScreen;
    private GameObject introText;
    private GameObject finalScreen;
    private GameObject apartmentDark;
    private GameObject ldpd;
    private GameObject street;
    private GameObject inventory;

    private float timer = 0;

    public bool canLeaveLDPD = false;

    // Start is called before the first frame update
    void Start()
    {
        Screen.SetResolution(720, 460, false);
        black = GameObject.Find("Black").GetComponent<Transition>();
        introText = GameObject.Find("IntroText");
        apartmentDark = GameObject.Find("ApartmentDark");
        titleScreen = GameObject.Find("Title");
        finalScreen = GameObject.Find("FinalScreen");
        ldpd = GameObject.Find("LDPD");
        street = GameObject.Find("Street");
        inventory = GameObject.Find("Inventory");
        introText.SetActive(false);
        apartmentDark.SetActive(false);
        inventory.SetActive(false);
        finalScreen.SetActive(false);
        ldpd.SetActive(false);
        street.SetActive(false);
        hero = GameObject.Find("Hero");
        cursorManager = GameObject.Find("GameLogic").GetComponent<CursorManager>();
        LoadScene(currentScene);

        // DELETE THIS
        //InventoryManager inventoryManager = GameObject.Find("Inventory").GetComponent<InventoryManager>();
        //inventoryManager.AddToInventory(GameObject.Find("full-proof"));
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
        } else if (currentScene == Scene.Ending) {
            timer += Time.deltaTime;
            if (timer > 3) {
                LoadScene(Scene.FinalEnd);
            }
        }
    }

    public void LoadScene(Scene sceneName) {
        currentScene = sceneName;
        if (currentScene == Scene.TitleScreen) {
            GameObject.Find("Title").transform.position = Vector3.zero;
            Camera.main.GetComponent<AudioSource>().clip = introMusic;
            Camera.main.GetComponent<AudioSource>().Play();
            hero.SetActive(false);
        } else if (currentScene == Scene.IntroText) {
            titleScreen.SetActive(false);
            introText.SetActive(true);
            black.FadeOut();
        } else if (currentScene == Scene.IntroMurder) {
            Camera.main.GetComponent<AudioSource>().Stop();
            introText.SetActive(false);
            apartmentDark.SetActive(true);
            Camera.main.transform.position = new Vector3(-4, 0, -10);
            black.FadeOut();
        } else if (currentScene == Scene.IntroPoliceStation) {
            Camera.main.GetComponent<AudioSource>().clip = mainMusic;
            Camera.main.GetComponent<AudioSource>().Play();
            apartmentDark.SetActive(false);
            hero.SetActive(true);
            ldpd.SetActive(true);
            street.SetActive(true);
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
                        "Chief McFly just called me in her office.",
                        "Must be about the raise she keeps telling me about!"
                    }
                }
                });
            black.FadeOut();
        } else if (currentScene == Scene.Ending) {
            timer = 0;
            black.FadeIn(() => {
                hero.SetActive(false);
                street.SetActive(false);
                inventory.SetActive(false);
            });
        } else if (currentScene == Scene.FinalEnd) {
            Camera.main.transform.position = new Vector3(0, 0, -10);
            finalScreen.SetActive(true);
            black.FadeOut();
            Camera.main.GetComponent<AudioSource>().clip = introMusic;
            Camera.main.GetComponent<AudioSource>().Play();
        }

    }
}
