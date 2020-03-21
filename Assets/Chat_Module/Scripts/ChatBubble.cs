using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatBubble : MonoBehaviour
{

    public Text message;
    public Text name;
    public Image panelColor;

    public void Initialize(string _name, string _message, ChatMessageType type)
    {
        if (type == ChatMessageType.Friend)
        {
            panelColor.color = Color.blue;
            message.alignment = TextAnchor.MiddleLeft;
            name.alignment = TextAnchor.MiddleLeft;
        }
        else if (type == ChatMessageType.Owner)
        {
            panelColor.color = Color.green;
            message.alignment = TextAnchor.MiddleRight;
            name.alignment = TextAnchor.MiddleRight;
        }
        name.text = _name;
        message.text = _message;
    }
}

public enum ChatMessageType
{
    None,
    Owner,
    Friend
}
