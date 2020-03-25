using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SendBird;

public class ProfileScreen : MonoBehaviour
{

    public Text _name;
    public Text city;
    public Text info;
    public Text friend;
    public Text post;

    public InputField p_story;
    public InputField p_city;
    public GameObject popup;

    private void OnEnable()
    {
        _name.text = AppManager.Instance.Name;
        city.text = AppManager.Instance.city;
        info.text = AppManager.Instance.story;
        friend.text = "Friends \n" + Chatmanager.Instance.friendsUserList.Count;
        post.text = "Posts \n" + GetBlogCount();
        if (string.IsNullOrWhiteSpace(AppManager.Instance.city))
            city.text = "City";
        if (string.IsNullOrWhiteSpace(AppManager.Instance.story))
            info.text = "Write your info...";
        //“One of the big things in my intense outpatient therapy ... was keeping a journal. Believe it or not, that was very therapeutic.You start seeing all these little things start adding up — all of these little lifestyle changes. You have to find what works for you. … Keep your network going... Keep interacting.The more you put out in there that’s positive, the more that’s going to come back to you.”
    }

    private int GetBlogCount()
    {
        int count = 0;
        List<BaseMessage> postsList = new List<BaseMessage>();
        postsList = Chatmanager.Instance.blogHistrory;
        int limit = 0;
        if (postsList.Count > 10)
        {
            limit = 10;
        }
        else
        {
            limit = postsList.Count;
        }
        for (int i = 0; i < limit; i++)
        {
            UserMessage t = (UserMessage)postsList[i];
            if (t.Sender.Nickname.Equals(Chatmanager.Instance.userName))
            {
                count++;
            }
        }
        return count;
    }

    public void SaveInfo()
    {
        AppManager.Instance.story = p_story.text;
        AppManager.Instance.city = p_city.text;
        AppManager.Instance.SetPlayerPrefs();
        city.text = AppManager.Instance.city;
        info.text = AppManager.Instance.story;
        DeactivatePopup();
    }

    public void ActivatePopup()
    {
        popup.SetActive(true);
    }

    public void DeactivatePopup()
    {
        popup.SetActive(false);
    }
}
