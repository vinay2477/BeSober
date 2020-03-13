using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserPreferenceShell : MonoBehaviour
{
    public Text interest;
    // Use this for initialization
    void OnEnable()
    {
        interest.text = "";
    }

    // Update is called once per frame
    public void Cancel()
    {
        AppManager.Instance.RemoveInterest(interest.text);
    }

    public void SetName(string s)
    {
        interest.text = s;
    }
}
