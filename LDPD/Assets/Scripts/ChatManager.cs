using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChatManager : MonoBehaviour
{
    public GameObject chat;
    public GameObject bubble;
    public GameObject text;
    private bool isActive = false;
    private int conversationIndex = 0;
    private int chatIndex = 0;
    private Conversation.ChatEntry[] conversation;

    // Start is called before the first frame update
    void Start()
    {
        if (!isActive) chat.SetActive(false);
    }
    
    public bool IsActive() {
        return isActive;
    }

    public void Next() {
        if (chatIndex < conversation[conversationIndex].text.Length - 1) {
            chatIndex++;
            Debug.Log("Next Chat");
            RefreshChat();
        } else if (conversationIndex < conversation.Length - 1) {
            conversationIndex++;
            chatIndex = 0;
            RefreshChat();
        } else {
            chatIndex = 0;
            conversationIndex = 0;
            isActive = false;
            chat.SetActive(false);
        }
    }

    public void StartChat(Conversation.ChatEntry[] conversation) {
        this.conversation = conversation;
        isActive = true;
        chat.SetActive(true);
        chatIndex = 0;
        conversationIndex = 0;
        RefreshChat();
    }

    public void RefreshChat() {
        float xPos = Camera.main.transform.position.x - conversation[conversationIndex].speaker.transform.position.x;
        Debug.Log(xPos);
        if (xPos >= 0) {
            xPos = -195 * xPos - 40;
        } else {
            xPos = 195 * xPos + 100;
        }
        bubble.GetComponent<RectTransform>().transform.localPosition = new Vector3(xPos, 165, 0);
        text.GetComponent<TextMeshProUGUI>().text = conversation[conversationIndex].text[chatIndex];
    }

}
