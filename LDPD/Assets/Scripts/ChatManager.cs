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
        float diff = Camera.main.transform.position.x - conversation[conversationIndex].speaker.transform.position.x;
        float pxDiff = Mathf.Abs(diff) * 360 / 1.8f;
        float xPos = 0f;
        if (diff > 0) {
            xPos = pxDiff - 240;
        } else if (diff < 0) {
            xPos = pxDiff - 80;
        } else {
            xPos = pxDiff - 60;
        }
        if (xPos > 240) xPos = 240;
        if (xPos < -240) xPos = -240;
        bubble.GetComponent<RectTransform>().transform.localPosition = new Vector3(xPos, 165, 0);
        text.GetComponent<TextMeshProUGUI>().text = conversation[conversationIndex].text[chatIndex];
    }

}
