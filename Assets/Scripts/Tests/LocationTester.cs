using UnityEngine;

public class LocationTester : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            var service = ServiceLocator.GetService<LocationService>().GetLocationInfo;
            Debug.Log("Location: " + service.latitude + " " + service.longitude + " " + service.altitude + " " + service.horizontalAccuracy + " " + service.timestamp);
        }
    }
}
