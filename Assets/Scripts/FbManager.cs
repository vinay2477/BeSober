using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Facebook.MiniJSON;
using Facebook.Unity;

public class FbManager : MonoBehaviour {
	
	public string get_data;
	public string fbname;

	// Awake function from Unity's MonoBehavior
	void Awake ()
	{
		if (!FB.IsInitialized) {
			// Initialize the Facebook SDK
			FB.Init(InitCallback, OnHideUnity);
		} else {
			// Already initialized, signal an app activation App Event
			FB.ActivateApp();
		}
	}

	private void InitCallback ()
	{
		if (FB.IsInitialized) {
			// Signal an app activation App Event
			FB.ActivateApp();
			// Continue with Facebook SDK
			// ...
		} else {
			Debug.Log("Failed to Initialize the Facebook SDK");
		}
	}

	private void OnHideUnity (bool isGameShown)
	{
		if (!isGameShown) {
			// Pause the game - we will need to hide
			Time.timeScale = 0;
		} else {
			// Resume the game - we're getting focus again
			Time.timeScale = 1;
		}
	}


	public void FBLogin()
	{
		var perms = new List<string>(){"public_profile", "email"};
		FB.LogInWithReadPermissions(perms, AuthCallback);	
	}

	private void AuthCallback (ILoginResult result) {
		if (FB.IsLoggedIn) {
			// AccessToken class will have session details
			var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
			// Print current access token's User ID
			Debug.Log(aToken.UserId);
			playerID = aToken.UserId.ToString();
			FB.API("/me?fields=name,email", Facebook.Unity.HttpMethod.GET, GetFacebookData);
			// Print current access token's granted permissions
			foreach (string perm in aToken.Permissions) {
				Debug.Log(perm);
			}
		} else {
			Debug.Log("User cancelled login");
		}
	}

	void GetFacebookData(Facebook.Unity.IGraphResult result)
	{
		string fbName = result.ResultDictionary["name"].ToString();
		string email = result.ResultDictionary["email"].ToString();
		signupApi = new SignupApi ();
		signupApi.user_name = fbName;
		signupApi.user_email = email;
		Debug.Log("fbName: " + fbName);
		Debug.Log("email: " + email);
		StartCoroutine ("Signup");
	}

	SignupApi signupApi;
	string playerID;

	IEnumerator Signup ()
	{	
		string infoText = JsonUtility.ToJson (signupApi).ToString ();

		WWWForm form = new WWWForm ();
		form.AddField ("uid", playerID);
		form.AddField ("info", infoText);

		using (UnityWebRequest www = UnityWebRequest.Post ("https://www.zhfbaa.cn/api/addict/signin?appid=addict&access_token=0000&sign=12345&uid=", form)) {
			yield return www.SendWebRequest ();

			if (www.isNetworkError || www.isHttpError) {
				Debug.Log (www.error);
			} else {
				string responseText = www.downloadHandler.text;

				SignupCallBack callback = new SignupCallBack ();
				callback = JsonUtility.FromJson<SignupCallBack> (responseText);

				if (callback.errmsg == "OK") {
					AppManager.Instance.phoneNumber =playerID;
					ScreenManager.Instance.Activate<HomeScreen> ();
				} else if (callback.errmsg == "wrong username or userpwd") {
					
				}
			}
		}
	}
}
