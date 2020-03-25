using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommunityScreen : PanelBase
{
    public GameObject connect;
    public GameObject chat;
    public GameObject blog;
    public GameObject profile;

    public void GoToHome()
    {
        ScreenManager.Instance.Activate<HomeScreen>();
    }

    public void ShowConnectScreen()
    {
        chat.gameObject.SetActive(false);
        blog.gameObject.SetActive(false);
        profile.gameObject.SetActive(false);
        connect.gameObject.SetActive(true);
    }

    public void ShowChat()
    {
        blog.gameObject.SetActive(false);
        profile.gameObject.SetActive(false);
        connect.gameObject.SetActive(false);
        chat.gameObject.SetActive(true);
    }

    public void ShowBlog()
    {
        connect.gameObject.SetActive(false);
        chat.gameObject.SetActive(false);
        profile.gameObject.SetActive(false);
        blog.gameObject.SetActive(true);
    }

    public void ShowProfile()
    {
        connect.gameObject.SetActive(false);
        chat.gameObject.SetActive(false);
        blog.gameObject.SetActive(false);
        profile.gameObject.SetActive(true);
    }

    public void ShowHome()
    {
        connect.gameObject.SetActive(false);
        chat.gameObject.SetActive(false);
        blog.gameObject.SetActive(false);
        profile.gameObject.SetActive(false);
    }

}

