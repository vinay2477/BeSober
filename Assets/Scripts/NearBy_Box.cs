using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using SendBird;

public class NearBy_Box : MonoBehaviour
{

    private User user;
    public Text username;
    public Image dp;

    public void UserClicked()
    {
        Chatmanager.Instance.selectedUser = user;
    }

    public void AddFriend()
    {
        Chatmanager.Instance.selectedUser = user;
        Chatmanager.Instance.CreatePrivateChat();
    }

    public void Initialize(User _user)
    {
        user = _user;
        username.text = _user.Nickname;
        dp.sprite = Chatmanager.Instance.avatar[UnityEngine.Random.Range(0, Chatmanager.Instance.avatar.Count)];
    }
}
