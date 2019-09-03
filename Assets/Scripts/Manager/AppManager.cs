using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;

public class AppManager : MonoBehaviour {

	public static AppManager Instance;

	public string phoneNumber;
	public string Name;
	public string email;
	public string password;

	public GameObject loading;

	public void SetUserData(string phone, string name,string mail, string pwd)
	{
		phoneNumber = phone;
		Name = name;
		email = mail;
		password = pwd;
		SetPlayerPrefs ();
	}

	void Awake ()
	{
		//Check if instance already exists
		if (Instance == null) {
			//if not, set instance to this
			Instance = this;
		}
		//If instance already exists and it's not this:
		else if (Instance != this) {
			//Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
			Destroy (gameObject);    
		}
		//Sets this to not be destroyed when reloading scene
		DontDestroyOnLoad (gameObject);
	}

	void SetPlayerPrefs()
	{
		UserData data = new UserData ();
		data.email = email;
		data.phoneNumber = phoneNumber;
		data.Name = Name;
		data.password = password;
		data.isLoggedIn = true;

		PlayerPrefs.SetString ("SoberAI", JsonUtility.ToJson(data).ToString());
	}

	public void FetchUserData()
	{
		StartCoroutine ("GetUserData");
	}


	public GetUserCallBack callback;

	IEnumerator GetUserData()
	{
		GetUserApi userdata = new GetUserApi ();
		userdata.user_phone = phoneNumber;
		string infoText = JsonUtility.ToJson (userdata).ToString ();

		WWWForm form = new WWWForm ();
		form.AddField ("uid", phoneNumber);
		form.AddField ("info", infoText);

		using (UnityWebRequest www = UnityWebRequest.Post ("https://www.zhfbaa.cn/api/addict/signin?appid=addict&access_token=0000&sign=12345&uid=", form)) {
			yield return www.SendWebRequest ();

			if (www.isNetworkError || www.isHttpError) {
				Debug.Log (www.error);
			} else {
				string responseText = www.downloadHandler.text;
				Debug.Log (responseText);

				callback = new GetUserCallBack ();
				callback = JsonUtility.FromJson<GetUserCallBack> (responseText);
				Debug.Log (responseText);
				if (callback.errmsg == "OK") {
					Debug.Log (callback.info.user_phone);
					Debug.Log (callback.info.user_name);
					Debug.Log (callback.info.user_email);
					Debug.Log (callback.info.user_pwd);
					SetUserData (callback.info.user_phone, callback.info.user_name, callback.info.user_email, callback.info.user_pwd);

				} else{
					
				}
			}
		}
	}

}

[Serializable]
public class UserData{
	public string phoneNumber;
	public string Name;
	public string email;
	public string password;
	public bool isLoggedIn;
}

public class GetUserApi
{
	public string user_phone;
}

[Serializable]
public class GetUserCallBack
{
	public int errno;
	public string errmsg;
	public double logid;
	public Info info;
}

[Serializable]
public class Info
{
	public string _id;
	public int uid;
	public string appid;
	public string user_name;
	public string user_pwd;
	public string user_email;
	public string user_phone;
	public int _ctime;
	public int _mtime;
}
