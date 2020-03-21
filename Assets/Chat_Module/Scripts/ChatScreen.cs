using System.Collections;
using System.Collections.Generic;
using SendBird;
using UnityEngine;
using UnityEngine.UI;

public class ChatScreen : MonoBehaviour
{

    public GameObject chatObject;
    public GameObject chatObjectParent;

    public InputField message;

    // Use this for initialization
    void OnDisable()
    {
        //ClearList();
        Chatmanager.Instance.chatHistrory = new List<SendBird.BaseMessage>();
        Debug.Log(Chatmanager.Instance.chatHistrory.Count);
    }

    void OnEnable()
    {
        if (Chatmanager.Instance.chatHistrory.Count > 0)
            Chatmanager.Instance.chatHistrory.Reverse();
        ClearList();
        IntializeList();
    }

    void IntializeList()
    {
        for (int resultIndex = 0; resultIndex < Chatmanager.Instance.chatHistrory.Count; resultIndex++)
        {
            if (Chatmanager.Instance.chatHistrory[resultIndex] is UserMessage)
            {
                GameObject temp = Instantiate(chatObject.gameObject);
                temp.transform.SetParent(chatObjectParent.transform);
                temp.transform.localScale = Vector3.one;

                if (((UserMessage)Chatmanager.Instance.chatHistrory[resultIndex]).Sender.Nickname == Chatmanager.Instance.userName)
                {
                    temp.GetComponent<Chat_Panel_Bubble>().Initialize(((UserMessage)Chatmanager.Instance.chatHistrory[resultIndex]).Sender.Nickname, ((UserMessage)Chatmanager.Instance.chatHistrory[resultIndex]).Message, ChatMessageType.Owner);
                }
                else
                {
                    temp.GetComponent<Chat_Panel_Bubble>().Initialize(((UserMessage)Chatmanager.Instance.chatHistrory[resultIndex]).Sender.Nickname, ((UserMessage)Chatmanager.Instance.chatHistrory[resultIndex]).Message, ChatMessageType.Friend);
                }
            }
        }
    }

    public void AddChatMessage(BaseMessage message)
    {
        if (message is UserMessage)
        {
            GameObject temp = Instantiate(chatObject.gameObject);
            temp.transform.SetParent(chatObjectParent.transform);
            temp.transform.localScale = Vector3.one;

            if (((UserMessage)message).Sender.Nickname == Chatmanager.Instance.userName)
            {
                temp.GetComponent<Chat_Panel_Bubble>().Initialize(((UserMessage)message).Sender.Nickname, ((UserMessage)message).Message, ChatMessageType.Owner);
            }
            else
            {
                temp.GetComponent<Chat_Panel_Bubble>().Initialize(((UserMessage)message).Sender.Nickname, ((UserMessage)message).Message, ChatMessageType.Friend);
            }
        }
    }

    void ClearList()
    {
        for (int childIndex = chatObjectParent.transform.childCount - 1; childIndex >= 0; --childIndex)
        {
            Transform child = chatObjectParent.transform.GetChild(childIndex);
            child.SetParent(null);
            Destroy(child.gameObject);
        }
    }

    public void SendMessage()
    {
        if (!string.IsNullOrWhiteSpace(message.text))
        {
            Chatmanager.Instance.SendPrivateMessage(message.text);
            message.text = "";
        }
    }
}
