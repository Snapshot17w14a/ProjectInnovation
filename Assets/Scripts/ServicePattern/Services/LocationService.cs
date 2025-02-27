using UnityEngine.Android;
using System.Collections;
using UnityEngine;

public class LocationService : Service
{
    public LocationInfo GetLocationInfo => Input.location.lastData;

    private void Awake()
    {
        ServiceLocator.RegisterService<LocationService>(this);
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
        }
        StartCoroutine(StartTracking());
    }

    private IEnumerator StartTracking()
    {
        // Check if the user has location service enabled.
        if (!Input.location.isEnabledByUser)
            Debug.Log("Location not enabled on device or app does not have permission to access location");

        // Starts the location service.
        Input.location.Start();

        Debug.Log(Input.location.status);

        // Waits until the location service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // If the service didn't initialize in 20 seconds this cancels location service use.
        if (maxWait < 1)
        {
            Debug.Log("Timed out");
            Debug.Log(Input.location.status);
            yield break;
        }

        // If the connection failed this cancels location service use.
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.LogError("Unable to determine device location");
            yield break;
        }
        else
        {
            // If the connection succeeded, this retrieves the device's current location and displays it in the Console window.
            Debug.Log("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
        }
    }

    public void StopTracking()
    {
        // Stops the location service if there is no need to query location updates continuously.
        Input.location.Stop();
        ServiceLocator.UnregisterService<LocationService>(this);
    }
}
