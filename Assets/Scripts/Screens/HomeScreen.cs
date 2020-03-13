using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using Assets.SimpleAndroidNotifications;

public class HomeScreen : PanelBase
{
    public List<GraphNode> graphNode;
    public List<string> graphDateList;
    public List<int> valueList;

    public Text userGraphText;


    public GameObject popup;
    public GameObject popup1;
    public Text popupText;
    public Text popupText1;

    public GameObject graph;

    public InputField inputFieldBeer;
    public InputField inputFieldWine;
    public InputField inputFieldMalt;
    public InputField inputFieldHard;

    public GameObject PosObject;
    public GameObject NegObject;
    public GameObject EmotionTracker;


    void Awake()
    {
        userGraphText.gameObject.SetActive(true);
        inputFieldBeer.characterLimit = 2;
        inputFieldWine.characterLimit = 2;
        inputFieldMalt.characterLimit = 2;
        inputFieldHard.characterLimit = 2;

    }

    void OnEnable()
    {
        userGraphText.text = "Fetching Data...";
        ResetGraph();
        StartCoroutine("GetDoseData");


    }

    void Start()
    {

    }


    public void ActivatePopup(string data)
    {
        popup.SetActive(true);
        popupText.text = data;
    }

    public void ActivatePopup1(string data)
    {
        popup1.SetActive(true);
        popupText1.text = data;
    }


    public void DeactivatePopup()
    {
        popup.SetActive(false);
    }

    public void DeactivatePopup1()
    {
        popup1.SetActive(false);
    }


    void InitializeGraph()
    {
        graphDateList = new List<string>();
        DateTime currentday = DateTime.Now;
        graphDateList.Add(String.Format("{0:dd/MM}", currentday));

        DateTime date = currentday;
        for (int i = 0; i < 30; i++)
        {
            DateTime temp = date.AddDays(-1);
            graphDateList.Add(String.Format("{0:dd/MM}", temp));
            date = temp;
        }

        graphDateList.Reverse();

        for (int i = (graphNode.Count - 1); i >= 0; i--)
        {
            graphNode[i].UpdateNode(valueList[i], graphDateList[i].ToString());
        }
        userGraphText.gameObject.SetActive(false);
        graph.SetActive(true);
        CheckForLastSeen();
    }

    void ResetGraph()
    {
        calldose = new DoseDataCallBack();
        for (int i = (graphNode.Count - 1); i >= 0; i--)
        {
            graphNode[i].UpdateNode(0, "");
        }
        graph.SetActive(false);
    }

    public void GoToSettings()
    {
        ScreenManager.Instance.Activate<SettingScreen>();
    }

    public void GoToInterest()
    {
        ScreenManager.Instance.Activate<UserPreferenceScreen>();
    }

    public void AddDoseBeer()
    {
        if (inputFieldBeer.text == "")
        {
            return;
        }

        int Count = int.Parse(inputFieldBeer.text);
        if (Count == 0)
        {
            return;
        }
        StartCoroutine(UpdateDose(inputFieldBeer.text));
        inputFieldBeer.text = "";
    }

    public void AddDoseWine()
    {
        if (inputFieldWine.text == "")
        {
            return;
        }

        int Count = int.Parse(inputFieldWine.text);
        if (Count == 0)
        {
            return;
        }
        StartCoroutine(UpdateDose(inputFieldWine.text));
        inputFieldWine.text = "";
    }

    public void AddDoseMalt()
    {
        if (inputFieldMalt.text == "")
        {
            return;
        }

        int Count = int.Parse(inputFieldMalt.text);
        if (Count == 0)
        {
            return;
        }
        StartCoroutine(UpdateDose(inputFieldMalt.text));
        inputFieldMalt.text = "";
    }

    public void AddDoseHard()
    {
        if (inputFieldHard.text == "")
        {
            return;
        }

        int Count = int.Parse(inputFieldHard.text);
        if (Count == 0)
        {
            return;
        }
        StartCoroutine(UpdateDose(inputFieldHard.text));
        inputFieldHard.text = "";
    }

    private IEnumerator UpdateDose(string value)
    {
        AppManager.Instance.loading.SetActive(true);
        DoseClass dose = new DoseClass();
        dose.user_dose = value;

        string infoText = JsonUtility.ToJson(dose).ToString();

        WWWForm form = new WWWForm();
        form.AddField("uid", AppManager.Instance.phoneNumber);
        form.AddField("info", infoText);

        using (UnityWebRequest www = UnityWebRequest.Post("http://39.107.240.174/api/addict/input?appid=addict&access_token=0000&sign=12345", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                AppManager.Instance.loading.SetActive(false);
                ActivatePopup("Network Error!");
            }
            else
            {
                string responseText = www.downloadHandler.text;
                Debug.Log(responseText);
                DoseCallBack callback = new DoseCallBack();
                callback = JsonUtility.FromJson<DoseCallBack>(responseText);

                if (callback.errmsg == "OK")
                {
                    AppManager.Instance.loading.SetActive(false);
                    ResetGraph();
                    userGraphText.gameObject.SetActive(true);
                    StartCoroutine("GetDoseData");
                    ActivatePopup("Succcessfully Updated!");
                }
                else
                {
                    AppManager.Instance.loading.SetActive(false);
                    ActivatePopup("Network Error!");
                    ActivatePopup("Network Error. Please Try again!");
                }
            }
        }
    }

    private IEnumerator GetDoseData()
    {
        DateTime today = DateTime.Now;
        DateTime endday = today.AddDays(-31);

        DoseData dose = new DoseData();
        dose.end_time = String.Format("{0:dd-MM-yyyy}", today);
        dose.start_time = String.Format("{0:dd-MM-yyyy}", endday);

        string infoText = JsonUtility.ToJson(dose).ToString();
        WWWForm form = new WWWForm();
        form.AddField("uid", AppManager.Instance.phoneNumber);
        form.AddField("info", infoText);

        using (UnityWebRequest www = UnityWebRequest.Post("http://39.107.240.174/api/addict/getdose?appid=addict&access_token=0000&sign=12345", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                GetDoseData();
            }
            else
            {
                string responseText = www.downloadHandler.text;
                Debug.Log(responseText);
                DoseDataCallBack callback = new DoseDataCallBack();
                callback = JsonUtility.FromJson<DoseDataCallBack>(responseText);
                calldose = callback;
                if (callback.errmsg == "OK")
                {
                    if (callback.List.Count != 0)
                    {
                        List<int> temp = new List<int>();
                        valueList = new List<int>();
                        for (int i = 0; i < callback.List.Count; i++)
                        {
                            temp.Add(callback.List[i].dose);
                        }
                        temp.Reverse();
                        for (int i = 0; i < 31; i++)
                        {
                            valueList.Add(temp[i]);
                        }
                        valueList.Reverse();
                        InitializeGraph();
                    }
                }
                else
                {

                }
            }
        }

    }

    const int optimalDrinkLimit = 4;

    public void AnalyseUserPattern()
    {
        List<int> userdose = new List<int>();

        for (int i = 0; i < (valueList.Count - 1); i++)
        {
            userdose.Add(valueList[i]);
        }

        float sum = 0, avg, pres, score = 0;

        for (int i = 0; i < 20; i++)
        {
            sum += userdose[i];
        }

        avg = sum / 20;

        if (avg == 0)
        {
            for (int i = 22; i < 30; i++)
            {
                sum += userdose[i];
            }
            avg = sum / 10;

            if (avg > 4 || sum > 14)
            {
                SendNegativeNotification();
            }
            else if (avg < 4 && sum < 14)
            {
                SendPositiveNotification();
            }
            else
            {
                SendAverageNotification();
            }


        }
        else
        {
            for (int i = 20; i < 30; i++)
            {
                pres = avg - userdose[i];
                score += pres;
            }

            if (score > 10)
            {
                SendPositiveNotification();
            }
            else if (score < 10)
            {
                SendNegativeNotification();
            }
            else
            {
                SendAverageNotification();
            }
            // compare score value
        }
    }

    public void CheckForLastSeen()
    {
        Debug.Log(AppManager.Instance.userdata.lastSeen);
        Debug.Log(AppManager.Instance.userdata.lastUpdated);

        if (AppManager.Instance.lastSeen == null || AppManager.Instance.lastUpdated == null)
        {
            AppManager.Instance.lastSeen = DateTime.Today;
            AppManager.Instance.lastUpdated = DateTime.Today;
            AnalyseUserPattern();
        }
        else
        {
            if (DateTime.Today == AppManager.Instance.lastUpdated)
            {
                return;
            }
            else
            {
                if (DateTime.Today > AppManager.Instance.lastUpdated)
                {
                    AppManager.Instance.lastSeen = DateTime.Today;
                    AppManager.Instance.lastUpdated = DateTime.Today;
                    AnalyseUserPattern();
                }
            }
        }
        AppManager.Instance.lastSeen = DateTime.Today;
        AppManager.Instance.SetPlayerPrefs();
    }

    void SendPositiveNotification()
    {
        List<string> notifyText = new List<string>() { "Achievement! Your goal is not so far. You are almost there. Cheers!",
            "Wow! Breaking a bad habit is now your achievement.",
            "Do you think it's impossible? No.It's possible! Only if you stay and remind yourself impossible itself says I’m possibe!"
        };
        notify = notifyText[UnityEngine.Random.Range(0, notifyText.Count)];

        StartCoroutine("DelayedPopup");
    }

    void SendNegativeNotification()
    {
        List<string> notifyText = new List<string>() { "Hey, Keep reminding yourself. Nothing changes if NOTHING changes.",
            "Did you ever feel its too late? No  Its never too late! Replenish your goal.",
            "Addiction is not your fault buddy! But you Know? Recovery is your RESPONSIBILITY."
        };
        notify = notifyText[UnityEngine.Random.Range(0, notifyText.Count)];

        StartCoroutine("DelayedPopup");
    }

    void SendAverageNotification()
    {
        List<string> notifyText = new List<string>() { "Do you always think about your success? Then you always have to be a work in progress.",
            "Worrying? Again and Again, But you can make it possible.",
            "One step can make all the difference. Why don’t you try doing that?"
        };
        notify = notifyText[UnityEngine.Random.Range(0, notifyText.Count)];

        StartCoroutine("DelayedPopup");
    }

    string notify;

    IEnumerator DelayedPopup()
    {
        yield return new WaitForSeconds(5);
        ActivatePopup1(notify);
        notify = "";
    }

    public void ActivateObject(int Type)
    {
        switch ((BTType)Type)
        {
            case BTType.neg:
                {
                    NegObject.SetActive(true);
                    PosObject.SetActive(false);
                }
                break;
            case BTType.pos:
                {
                    PosObject.SetActive(true);
                    NegObject.SetActive(false);
                }
                break;
            case BTType.none:
                {
                    PosObject.SetActive(false);
                    NegObject.SetActive(false);
                }
                break;
            case BTType.emotionOpen:
                {
                    EmotionTracker.SetActive(true);
                }
                break;
            case BTType.emotionClose:
                {
                    AppManager.Instance.CalculateLatestFeedbackScore();
                    EmotionTracker.SetActive(false);
                    DeactivatePopup1();
                }
                break;

        }
    }


    public DoseDataCallBack calldose;
}

public class DoseClass
{
    public string user_dose;
}

public class DoseData
{
    public string start_time;
    public string end_time;
}

public class DoseCallBack
{
    public int errno;
    public string errmsg;
    public double logid;
}

[Serializable]
public class DoseDataCallBack
{
    public int errno;
    public string errmsg;
    public List<List> List;
    public long logid;
}

[Serializable]
public class List
{
    public int time;
    public int dose;
}
