using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SendBird;

public class UserObjecct : MonoBehaviour
{

    private User user;
    public Text username;

    public void UserClicked()
    {
        Chatmanager.Instance.selectedUser = user;
    }

    public void Initialize(User _user)
    {
        user = _user;
        username.text = _user.Nickname;
    }
}
