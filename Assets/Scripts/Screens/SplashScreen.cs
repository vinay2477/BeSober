﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SplashScreen : PanelBase {
	
	public Slider loading;
	float timeLeft = 0;
	public UserData data;

	void OnEnable()
	{
		timeLeft = 0;
		data = new UserData ();
		string	userData = PlayerPrefs.GetString ("SoberAI"); 
		if (!string.IsNullOrWhiteSpace (userData)) {
			data = JsonUtility.FromJson<UserData> (PlayerPrefs.GetString ("SoberAI"));
			AppManager.Instance.phoneNumber = data.phoneNumber;
		} else {
			data.isLoggedIn = false;
		}
	}

	void Awake()
	{
		//PlayerPrefs.DeleteAll ();

	}

	void Update () {
		timeLeft += Time.deltaTime;
		loading.value = timeLeft;
		if ( timeLeft > 5.0f )
		{
			if (data.isLoggedIn) {
				if (data.phoneNumber != null) {	
					AppManager.Instance.phoneNumber = data.phoneNumber;
					AppManager.Instance.FetchUserData ();
					ScreenManager.Instance.Activate<HomeScreen> ();

				} else {
					ScreenManager.Instance.Activate<LoginScreen> ();
				}

			} else {
				ScreenManager.Instance.Activate<LoginScreen> ();
			}
		}	
	}

}