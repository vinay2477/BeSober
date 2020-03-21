using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendsListScreen : MonoBehaviour
{

    public GameObject userObject;
    public GameObject userObjectParent;


    void OnEnable()
    {
        ClearList();
        IntializeList();
    }

    public void Refresh()
    {
        ClearList();
        IntializeList();
    }

    void IntializeList()
    {
        for (int resultIndex = 0; resultIndex < Chatmanager.Instance.friendsUserList.Count; resultIndex++)
        {
            GameObject temp = Instantiate(userObject.gameObject);
            temp.transform.SetParent(userObjectParent.transform);
            temp.transform.localScale = Vector3.one;
            temp.GetComponent<UserObjecct>().Initialize(Chatmanager.Instance.friendsUserList[resultIndex]);
        }
    }

    void ClearList()
    {
        for (int childIndex = userObjectParent.transform.childCount - 1; childIndex >= 0; --childIndex)
        {
            Transform child = userObjectParent.transform.GetChild(childIndex);
            child.SetParent(null);
            Destroy(child.gameObject);
        }
    }

    public void ConnectUser()
    {
        if (Chatmanager.Instance.selectedUser != null)
            Chatmanager.Instance.LoadPreviousChatHistory();
        Chatmanager.Instance.selectedUser = null;
    }

    public void Unfriend()
    {
        if (Chatmanager.Instance.selectedUser != null)
            Chatmanager.Instance.UnFriend();
        Chatmanager.Instance.selectedUser = null;
    }
}
