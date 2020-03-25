using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Post_Obj : MonoBehaviour
{

    public Text message;
    public Text city;
    public Image dp;
    public Text name;

    public void Initialize(string _name, string _message)
    {
        if (_name.Equals(Chatmanager.Instance.userName))
        {
            dp.sprite = Chatmanager.Instance.user;
        }
        else
        {
            dp.sprite = Chatmanager.Instance.avatar[UnityEngine.Random.Range(0, Chatmanager.Instance.avatar.Count)];
        }

        if (_name.Equals(Chatmanager.Instance.userName))
        {
            name.text = Chatmanager.Instance.userName;
            city.text = AppManager.Instance.city;
        }
        else
        {
            name.text = _name;
        }
        message.text = _message;
    }
}
