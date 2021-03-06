﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SplashScreen : PanelBase
{

    public Slider loading;
    float timeLeft = 0;
    public UserData data;

    void OnEnable()
    {
        data = new UserData();
        string userData = PlayerPrefs.GetString("SoberAI");
        if (!string.IsNullOrWhiteSpace(userData))
        {
            data = JsonUtility.FromJson<UserData>(PlayerPrefs.GetString("SoberAI"));
            AppManager.Instance.phoneNumber = data.phoneNumber;
        }
        else
        {
            data.isLoggedIn = false;
        }
    }

    void Awake()
    {
        //PlayerPrefs.DeleteAll ();

    }

    void Update()
    {
        timeLeft += Time.deltaTime;
        loading.value = timeLeft;
        if (timeLeft > 3.0f)
        {
            if (data.isLoggedIn)
            {
                if (!string.IsNullOrEmpty(data.phoneNumber))
                {
                    AppManager.Instance.userdata = data;
                    AppManager.Instance.phoneNumber = data.phoneNumber;
                    AppManager.Instance.story = data.story;
                    AppManager.Instance.city = data.city;
                    AppManager.Instance.FetchUserData();
                    AppManager.Instance.lastSeen = DateTime.Parse(data.lastSeen);
                    AppManager.Instance.lastUpdated = DateTime.Parse(data.lastUpdated);
                    AppManager.Instance.lastFeedback = DateTime.Parse(data.lastFeedback);
                    AppManager.Instance.latestFeedbackScore = data.latestFeedbackScore;
                    WrapperInterest temp = data.interest;
                    AppManager.Instance.UserInterest = temp.interest;
                    Chatmanager.Instance.userName = AppManager.Instance.userdata.Name;
                    Chatmanager.Instance.StartConnection();
                    ScreenManager.Instance.Activate<HomeScreen>();

                }
                else
                {
                    ScreenManager.Instance.Activate<LoginScreen>();
                    AppManager.Instance.userdata = new UserData();
                }

            }
            else
            {
                ScreenManager.Instance.Activate<LoginScreen>();
            }
        }
    }

}
