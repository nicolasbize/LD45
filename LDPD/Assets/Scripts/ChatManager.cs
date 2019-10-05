using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatManager : MonoBehaviour
{
    public GameObject chat;
    public bool isActive = false;

    // Start is called before the first frame update
    void Start()
    {
        chat = GameObject.Find("Chat");
        chat.SetActive(false);
    }
    
    public bool IsActive() {
        return isActive;
    }

    public void StartChat(Conversation.ChatEntry[] conversation) {
        chat.SetActive(true);

    }

}
