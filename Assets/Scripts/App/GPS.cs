using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPS : MonoBehaviour
{ //experimental!

    #region vars

    private float latitude;
    private float longitude;

    public float Latitude { get { return latitude; } private set { latitude = value; } }
    public float Longitude { get { return longitude; } private set { longitude = value; } }

    private bool gpsStart;

    private static GPS instance;
    public static GPS Instance { get { return instance; } private set { instance = value; } }

    #endregion

    #region init

    void Awake()
    {
        Instance = Instance == null ? this : Instance; //set instance

        StartCoroutine(StartGPS()); //start gps service
    }

    IEnumerator StartGPS()
    {
        if (!Input.location.isEnabledByUser)
        {
            Debug.Log("GPS disabled!");
            yield break;
        }

        Input.location.Start();

        int maxWait = 20;

        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1f);
            maxWait--; //decrement
        }

        if (maxWait <= 0)
        {
            Debug.Log("Timed out!");
            yield break;
        }
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.Log("Unable to use location services!");
            yield break;
        }

        UpdateCoordinates(); //init update

        gpsStart = true;
    }

    #endregion

    #region sm

    void Update()
    {
        if (!gpsStart) return;

        UpdateCoordinates();
    }

    #endregion

    #region methods

    private void UpdateCoordinates() {
        longitude = Input.location.lastData.longitude;
        latitude = Input.location.lastData.latitude;
    }

    #endregion
}
