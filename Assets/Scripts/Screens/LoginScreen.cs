using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class LoginScreen : PanelBase {

	public InputField phone;
	public InputField password;
	public Text invalidText;

	LoginApi loginapi;

	void Awake()
	{
		loginapi = new LoginApi ();
		invalidText.text = "";
	}

	public void LoginButton()
	{
		if (phone.text == "") {
			invalidText.text = "Enter valid Phone";
			return;
		}

		if (password.text == "") {
			invalidText.text = "Enter valid Password";
			return;
		} else {
			StartCoroutine ("Login");
		}
	}

	public void GoToSignUp()
	{
		ScreenManager.Instance.Activate<SignipScreen> ();
	}

	IEnumerator Login()
	{
		loginapi.user_phone = phone.text;
		loginapi.user_pwd = password.text;

		string infoText = JsonUtility.ToJson(loginapi).ToString();

		WWWForm form = new WWWForm();
		form.AddField("uid", phone.text);
		form.AddField("info", infoText);

		using (UnityWebRequest www = UnityWebRequest.Post("https://www.zhfbaa.cn/api/addict/login?appid=addict&access_token=0000&sign=12345", form))
		{
			yield return www.SendWebRequest();

			if (www.isNetworkError || www.isHttpError)
			{
				Debug.Log(www.error);
			}
			else
			{
				string responseText = www.downloadHandler.text;

				LoginCallBack callback = new LoginCallBack ();
				callback = JsonUtility.FromJson<LoginCallBack> (responseText);

				if (callback.errmsg == "OK") {
					invalidText.text = "";
					ScreenManager.Instance.Activate<HomeScreen> ();
				} else if (callback.errmsg ==  "wrong username or userpwd") {
					invalidText.text = "Invalid Username or Password!";
				}
			}
		}
	}
}

public class LoginApi
{
	public string user_pwd;
	public string user_phone;
}

public class LoginCallBack{
	public int errno;
	public string errmsg;
	public double logid; 
}
