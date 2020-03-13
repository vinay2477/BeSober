using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropdownElement : MonoBehaviour
{
    public Text dropdownText;

    // Update is called once per frame
    public void AddElement()
    {
        AppManager.Instance.AddInterest(dropdownText.text);
    }

    public void AddText(string s)
    {
        dropdownText.text = s;
    }
}
