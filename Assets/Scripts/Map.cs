using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mapbox.Examples;

public class Map : MonoBehaviour
{

    public GameObject screen1;
    public GameObject mapobject;
    public GameObject _camera;

    public SpawnOnMap _map;
    public Text location;
    public float lat = 33.583064f;
    public float longi = -101.8777325f;

    void SaveCo()
    {
        PlayerPrefs.SetFloat("lat", lat);
        PlayerPrefs.SetFloat("longi", longi);
    }

    void Start()
    {
        lat = 33.583064f;
        longi = -101.8777325f;
        SaveCo();
        StartCoroutine(GetLocation());
    }

    IEnumerator GetLocation()
    {
        // First, check if user has location service enabled
        if (!Input.location.isEnabledByUser)
            yield break;

        // Start service before querying location
        Input.location.Start();

        // Wait until service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // Service didn't initialize in 20 seconds
        if (maxWait < 1)
        {
            Debug.Log("Timed out");
            location.text = "Timed out";
            yield break;
        }

        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.Log("Unable to determine device location");
            location.text = "Unable to determine device location";
            yield break;
        }
        else
        {
            // Access granted and location value could be retrieved
            location.text = "Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp;
            lat = Input.location.lastData.latitude;
            longi = Input.location.lastData.longitude;
            SaveCo();
            Debug.Log("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
        }

        // Stop service if there is no need to query location updates continuously
        Input.location.Stop();
    }

    public void ShowHome()
    {

        _camera.SetActive(true);
        screen1.SetActive(true);
        mapobject.SetActive(false);

    }

    public void ChangeMap()
    {
        _camera.SetActive(false);
        screen1.SetActive(false);
        mapobject.SetActive(true);

    }

}
