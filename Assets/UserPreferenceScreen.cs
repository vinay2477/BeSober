using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserPreferenceScreen : MonoBehaviour
{

    public InputField inputField;
    public RectTransform resultsParent;
    public RectTransform prefab;

    void Awake()
    {
        //inputField.onValueChanged.AddListener(OnInputValueChanged);
    }

    public void OnInputValueChanged()
    {
        ClearResults();
        FillResults(GetResults(inputField.text));
    }

    private void ClearResults()
    {
        // Reverse loop since you destroy children
        for (int childIndex = resultsParent.childCount - 1; childIndex >= 0; --childIndex)
        {
            Transform child = resultsParent.GetChild(childIndex);
            child.SetParent(null);
            Destroy(child.gameObject);
        }
    }

    private void FillResults(List<string> results)
    {
        if (results != null)
        {
            for (int resultIndex = 0; resultIndex < results.Count; resultIndex++)
            {
                RectTransform child = Instantiate(prefab) as RectTransform;
                child.GetComponentInChildren<Text>().text = results[resultIndex];
                child.SetParent(resultsParent);
            }
        }
    }


    private List<string> GetResults(string input)
    {
        List<string> mockData = new List<string>() { "Paris", "Madrid", "London", "Rome", "Brussels", "Athens", "Dublin", "Lisbon", "Amsterdam", "Luxembourg" };

        if (!string.IsNullOrEmpty(input))
        {
            return mockData.FindAll((str) => str.IndexOf(input) >= 0);
        }
        return null;
    }

}