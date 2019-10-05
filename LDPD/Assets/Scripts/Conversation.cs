using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conversation : ScriptableObject {

    [System.Serializable]
    public class ChatEntry {
        public GameObject speaker;
        public string[] text;
    }

    public ChatEntry[] chatEntries;
}
