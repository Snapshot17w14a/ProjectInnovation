using UnityEngine;

public class CompassService : Service
{
    private float degrees;
    public void Awake()
    {
        StartCompass();
    }

    public float ReturnCompassDegrees()
    {
        if (!Input.compass.enabled)
        {
            Debug.LogError("Compass is not enabled");
            return 0;
        }

        degrees = Input.compass.magneticHeading;
        return degrees;
    }

    public void StartCompass()
    {
        Input.compass.enabled = true;
    }

    public void StopCompass()
    {
        Input.compass.enabled = false;
    }
}
