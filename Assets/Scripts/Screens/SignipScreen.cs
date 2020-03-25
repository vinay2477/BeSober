using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class SignipScreen : PanelBase
{

    public InputField name;
    public InputField email;
    public InputField phone;
    public InputField password;
    public InputField repassword;
    public Text invalidText;

    SignupApi signupApi;

    void OnEnable()
    {
        phone.text = "";
        password.text = "";
        name.text = "";
        email.text = "";
        repassword.text = "";
    }

    void Awake()
    {
        signupApi = new SignupApi();
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

        if (phone.text == "")
        {
            invalidText.text = "Enter valid Phone";
            return;
        }

        if (password.text == password.text)
        {
            StartCoroutine("Signup");
        }
        else
        {
            invalidText.text = "Password does not match";
            return;
        }
    }

    public void GoToLogin()
    {
        ScreenManager.Instance.Activate<LoginScreen>();
    }

    IEnumerator Signup()
    {
        AppManager.Instance.loading.SetActive(true);
        signupApi.user_name = name.text;
        signupApi.user_pwd = password.text;
        signupApi.user_email = email.text;
        signupApi.user_phone = phone.text;

        string infoText = JsonUtility.ToJson(signupApi).ToString();

        WWWForm form = new WWWForm();
        form.AddField("uid", phone.text);
        form.AddField("info", infoText);

        using (UnityWebRequest www = UnityWebRequest.Post("http://39.107.240.174/api/addict/signin?appid=addict&access_token=0000&sign=12345&uid=", form))
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

                SignupCallBack callback = new SignupCallBack();
                callback = JsonUtility.FromJson<SignupCallBack>(responseText);
                Debug.Log(responseText);
                if (callback.errmsg == "OK")
                {
                    invalidText.text = "";
                    AppManager.Instance.SetUserData(callback.info.user_phone, callback.info.user_name, callback.info.user_email, callback.info.user_pwd);
                    AppManager.Instance.loading.SetActive(false);
                    Chatmanager.Instance.userName = AppManager.Instance.userdata.Name;
                    Chatmanager.Instance.StartConnection();
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
}

public class SignupApi
{
    public string user_name;
    public string user_pwd;
    public string user_email;
    public string user_phone;
}

public class SignupCallBack
{
    public int errno;
    public string errmsg;
    public double logid;
    public Info info;
}