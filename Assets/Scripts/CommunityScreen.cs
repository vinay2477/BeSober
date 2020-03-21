using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommunityScreen : PanelBase
{
    public GameObject connect;
    public GameObject chat;

    public void GoToHome()
    {
        ScreenManager.Instance.Activate<HomeScreen>();
    }

    public void ShowConnectScreen()
    {
        connect.gameObject.SetActive(true);
        chat.gameObject.SetActive(false);
    }

    public void ShowChat()
    {
        connect.gameObject.SetActive(false);
        chat.gameObject.SetActive(true);
    }

    public void ShowHome()
    {
        connect.gameObject.SetActive(false);
        chat.gameObject.SetActive(false);
    }

}

