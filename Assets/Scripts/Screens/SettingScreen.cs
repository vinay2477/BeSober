using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class SettingScreen : PanelBase
{

    public InputField name;
    public InputField email;
    public InputField password;
    public InputField repassword;
    public Text invalidText;

    UpdateApi signupApi;

    void OnEnable()
    {
        password.text = "";
        name.text = "";
        email.text = "";
        repassword.text = "";
    }

    void Awake()
    {
        signupApi = new UpdateApi();
        invalidText.text = "";
    }

    public void RegisterButton()
    {

        if (name.text == "")
        {
            invalidText.text = "Enter valid Name";
            return;
        }

        if (password.text == "")
        {
            invalidText.text = "Enter valid Password";
            return;
        }

        if (email.text == "")
        {
            invalidText.text = "Enter valid Email ID";
            return;
        }

        if (password.text == password.text)
        {
            StartCoroutine("UpdateProfile");
        }
        else
        {
            invalidText.text = "Password does not match";
            return;
        }
    }

    public void GoToHome()
    {
        ScreenManager.Instance.Activate<HomeScreen>();
    }

    IEnumerator UpdateProfile()
    {
        AppManager.Instance.loading.SetActive(true);
        signupApi.user_name = "";
        signupApi.user_pwd = password.text;
        signupApi.user_email = email.text;

        string infoText = JsonUtility.ToJson(signupApi).ToString();

        WWWForm form = new WWWForm();
        form.AddField("uid", AppManager.Instance.phoneNumber);
        form.AddField("info", infoText);

        using (UnityWebRequest www = UnityWebRequest.Post("http://39.107.240.174/api/addict/update?appid=addict&access_token=0000&sign=12345&uid=", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                AppManager.Instance.loading.SetActive(false);
            }
            else
            {
                string responseText = www.downloadHandler.text;
                UpdateCallBack callback = new UpdateCallBack();
                callback = JsonUtility.FromJson<UpdateCallBack>(responseText);

                if (callback.errmsg == "OK")
                {
                    invalidText.text = "";
                    AppManager.Instance.FetchUserData();
                    AppManager.Instance.loading.SetActive(false);
                    ScreenManager.Instance.Activate<HomeScreen>();
                }
                else if (callback.errmsg == "wrong username or userpwd")
                {
                    AppManager.Instance.loading.SetActive(false);
                    invalidText.text = "Invalid Username or Password!";
                }
            }
        }
    }

    public void Logout()
    {
        AppManager.Instance.email = "";
        AppManager.Instance.Name = "";
        AppManager.Instance.phoneNumber = "";
        AppManager.Instance.password = "";
        PlayerPrefs.DeleteKey("SoberAI");
        ScreenManager.Instance.Activate<SplashScreen>();
    }
}

public class UpdateApi
{
    public string user_name;
    public string user_pwd;
    public string user_email;
}

public class UpdateCallBack
{
    public int errno;
    public string errmsg;
    public double logid;
}
