using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screenmanager : MonoBehaviour
{

    public GameObject home;
    public GameObject peopleNearBy;
    public GameObject friendsList;
    public GameObject chat;

    public void ShowHome()
    {
        Chatmanager.Instance.selectedUser = null;
        home.gameObject.SetActive(true);
        peopleNearBy.gameObject.SetActive(false);
        friendsList.gameObject.SetActive(false);
        chat.gameObject.SetActive(false);
    }

    public void ShowFriendsList()
    {
        home.gameObject.SetActive(false);
        peopleNearBy.gameObject.SetActive(false);
        friendsList.gameObject.SetActive(true);
        chat.gameObject.SetActive(false);
    }

    public void ShowNearByFriends()
    {
        home.gameObject.SetActive(false);
        peopleNearBy.gameObject.SetActive(true);
        friendsList.gameObject.SetActive(false);
        chat.gameObject.SetActive(false);
    }

    public void ShowChat()
    {
        home.gameObject.SetActive(false);
        peopleNearBy.gameObject.SetActive(false);
        friendsList.gameObject.SetActive(false);
        chat.gameObject.SetActive(true);
    }

}
