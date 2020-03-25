using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ConnectScreen : MonoBehaviour
{

    public GameObject nearByObject;
    public GameObject nearByObjectParent;

    public GameObject friendObject;
    public GameObject friendObjectParent;

    public GameObject popup;
    public Text name;
    public Text info;

    [SerializeField]
    public List<TherapistInfo> ListTherapist;

    public bool isConnect;

    void IntializeNearByObjectList()
    {
        for (int resultIndex = 0; resultIndex < Chatmanager.Instance.nearByPeople.Count; resultIndex++)
        {
            GameObject temp = Instantiate(nearByObject.gameObject);
            temp.transform.SetParent(nearByObjectParent.transform);
            temp.transform.localScale = Vector3.one;
            temp.GetComponent<NearBy_Box>().Initialize(Chatmanager.Instance.nearByPeople[resultIndex]);
        }
    }

    void ClearNearByObjectList()
    {
        for (int childIndex = nearByObjectParent.transform.childCount - 1; childIndex >= 0; --childIndex)
        {
            Transform child = nearByObjectParent.transform.GetChild(childIndex);
            child.SetParent(null);
            Destroy(child.gameObject);
        }
    }

    void IntializeFriendList()
    {
        for (int resultIndex = 0; resultIndex < Chatmanager.Instance.friendsUserList.Count; resultIndex++)
        {
            GameObject temp = Instantiate(friendObject.gameObject);
            temp.transform.SetParent(friendObjectParent.transform);
            temp.transform.localScale = Vector3.one;
            temp.GetComponent<FriendBox>().Initialize(Chatmanager.Instance.friendsUserList[resultIndex]);
        }
    }

    void ClearFriendList()
    {
        for (int childIndex = friendObjectParent.transform.childCount - 1; childIndex >= 0; --childIndex)
        {
            Transform child = friendObjectParent.transform.GetChild(childIndex);
            child.SetParent(null);
            Destroy(child.gameObject);
        }
    }

    public void RefreshFriends()
    {
        Debug.Log("inside friends :" + Chatmanager.Instance.friendsUserList.Count);
        ClearFriendList();
        IntializeFriendList();
    }

    public void RefreshNearBy()
    {

        Debug.Log("inside nearby :" + Chatmanager.Instance.nearByPeople.Count);
        ClearNearByObjectList();
        IntializeNearByObjectList();
    }

    void OnEnable()
    {
        isConnect = true;
        RefreshFriends();
        RefreshNearBy();
    }

    void OnDisable()
    {
        isConnect = false;
    }

    public void ClosePopup()
    {
        popup.SetActive(false);
    }

    public void InitializePopup(int index)
    {
        name.text = ListTherapist[index].name;
        info.text = ListTherapist[index].info;
        popup.SetActive(true);
    }

}

[Serializable]
public class TherapistInfo
{
    public string name;
    public string info;
}
