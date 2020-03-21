using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeopleNearBy : MonoBehaviour
{

    public GameObject userObject;
    public GameObject userObjectParent;


    void OnEnable()
    {
        ClearList();
        IntializeList();
    }

    void IntializeList()
    {
        for (int resultIndex = 0; resultIndex < Chatmanager.Instance.nearByPeople.Count; resultIndex++)
        {
            GameObject temp = Instantiate(userObject.gameObject);
            temp.transform.SetParent(userObjectParent.transform);
            temp.transform.localScale = Vector3.one;
            temp.GetComponent<UserObjecct>().Initialize(Chatmanager.Instance.nearByPeople[resultIndex]);
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
            Chatmanager.Instance.CreatePrivateChat();
        Chatmanager.Instance.selectedUser = null;
    }
}
