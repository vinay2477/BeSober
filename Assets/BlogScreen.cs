using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SendBird;

public class BlogScreen : MonoBehaviour
{

    public GameObject blogObject;
    public GameObject blogObjectParent;

    public List<BaseMessage> postsList;

    public InputField message;

    // Use this for initialization
    void OnDisable()
    {
    }

    void OnEnable()
    {

        postsList = new List<BaseMessage>();
        postsList = Chatmanager.Instance.blogHistrory;

        ClearList();
        IntializeList();
    }

    public void Refresh()
    {
        AppManager.Instance.loading.SetActive(true);
        ClearList();
        IntializeList();
        AppManager.Instance.loading.SetActive(false);
    }

    void IntializeList()
    {
        int limit = 0;
        if (postsList.Count > 10)
        {
            limit = 10;
        }
        else
        {
            limit = postsList.Count;
        }

        for (int resultIndex = 0; resultIndex < limit; resultIndex++)
        {
            GameObject temp = Instantiate(blogObject.gameObject);
            temp.transform.SetParent(blogObjectParent.transform);
            temp.transform.localScale = Vector3.one;
            UserMessage msg = (UserMessage)postsList[resultIndex];
            temp.GetComponent<Post_Obj>().Initialize(msg.Sender.Nickname, msg.Message);
        }
    }


    public void AddBlogPost(BaseMessage message)
    {
        if (message is UserMessage)
        {
            Chatmanager.Instance.blogHistrory.Insert(0, message);
            GameObject temp = Instantiate(blogObject.gameObject);
            temp.transform.SetParent(blogObjectParent.transform);
            temp.transform.SetSiblingIndex(0);
            temp.transform.localScale = Vector3.one;
            UserMessage msg = (UserMessage)message;
            temp.GetComponent<Post_Obj>().Initialize(msg.Sender.Nickname, msg.Message);

        }
    }

    void ClearList()
    {
        for (int childIndex = blogObjectParent.transform.childCount - 1; childIndex >= 0; --childIndex)
        {
            Transform child = blogObjectParent.transform.GetChild(childIndex);
            child.SetParent(null);
            Destroy(child.gameObject);
        }
    }

    public void SendMessage()
    {
        if (!string.IsNullOrWhiteSpace(message.text))
        {
            Chatmanager.Instance.SendBlogPost(message.text);
            message.text = "";
        }
    }
}
