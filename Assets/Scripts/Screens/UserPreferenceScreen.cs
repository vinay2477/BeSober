using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserPreferenceScreen : PanelBase
{

    public InputField inputField;
    public RectTransform resultsParent;
    public RectTransform prefab;
    public List<GameObject> interestBubble;
    public GameObject bubblePrefab;
    public GameObject bubbleParent;

    void OnEnable()
    {
        RefereshInterest();
    }

    public void OnInputValueChanged()
    {
        ClearResults();
        FillResults(GetResults(inputField.text));
    }

    public void GoToHome()
    {
        ScreenManager.Instance.Activate<HomeScreen>();
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
        List<string> mockData = new List<string>() { "Gym", "Buffet", "Concert", "Cafe or Bistro", "Pet Shop", "Fast Food", "Aerobics", "Music Shop", "Museum",
        "TV Show", "Food Trucks", "Games", "Super Market", "Restaurants", "Swimming", "Cycling", "Jewellery Store",
        "Running", "Circus", "Theater", "Cloth Shop", "Toy Shop", "Yoga", "Book Shop", "Exhibitions"};

        if (!string.IsNullOrEmpty(input))
        {
            return mockData.FindAll((str) => str.IndexOf(input) >= 0);
        }
        return null;
    }

    public void RemoveInterest(string s)
    {
        //for (int j = 0; j < interestBubble.Count; j++)
        //{
        //    if (interestBubble[j].GetComponent<UserPreferenceShell>().interest.Equals(s))
        //    {
        //        GameObject temp = interestBubble[j].gameObject;
        //        temp.transform.SetParent(null);
        //        Destroy(temp.gameObject);
        //        interestBubble.Remove(temp);
        //        break;
        //    }
        //}
        RefereshInterest();
    }

    public void RefereshInterest()
    {
        for (int j = 0; j < interestBubble.Count; j++)
        {
            interestBubble[j].transform.SetParent(null);
            Destroy(interestBubble[j].gameObject);
        }
        interestBubble.Clear();

        for (int i = 0; i < AppManager.Instance.UserInterest.Count; i++)
        {
            GameObject temp = Instantiate(bubblePrefab.gameObject);
            temp.transform.SetParent(bubbleParent.transform);
            temp.transform.localScale = new Vector3(0.7f, 0.7f, 1);
            temp.GetComponent<UserPreferenceShell>().SetName(AppManager.Instance.UserInterest[i]);
            interestBubble.Add(temp);
        }
    }

}