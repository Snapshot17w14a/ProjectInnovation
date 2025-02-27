using System.Collections;
using UnityEngine.UI;
using UnityEngine;

// Show WebCams and Microphones on an iPhone/iPad.
// Make sure NSCameraUsageDescription and NSMicrophoneUsageDescription
// are in the Info.plist.

public class TestingClass : MonoBehaviour
{
    //[SerializeField] private Image target;

    //private WebCamTexture webCamTexture;
    //[SerializeField] private AudioSource audioSource;
    //[SerializeField] private GameObject phone;
    //Gyroscope gyroscope;
    //Quaternion ninetydegx = Quaternion.Euler(-90, 0, 0);

    //IEnumerator Start()
    //{
    //    webCamTexture = new();
    //    FindWebCams();

    //    yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
    //    if (Application.HasUserAuthorization(UserAuthorization.WebCam))
    //    {
    //        Debug.Log("webcam found");
    //        webCamTexture.Play();
    //    }
    //    else
    //    {
    //        Debug.Log("webcam not found");
    //    }

    //    FindMicrophones();

    //    yield return Application.RequestUserAuthorization(UserAuthorization.Microphone);
    //    if (Application.HasUserAuthorization(UserAuthorization.Microphone))
    //    {
    //        Debug.Log("Microphone found");
    //    }
    //    else
    //    {
    //        Debug.Log("Microphone not found");
    //    }

    //    audioSource.clip = Microphone.Start(Microphone.devices[0], true, 10, 44100);
    //    audioSource.Play();

    //    gyroscope = Input.gyro;
    //    gyroscope.enabled = true;
    //}

    //private void Update()
    //{
    //    target.material.mainTexture = webCamTexture;
    //    //Debug.Log($"x: {Input.acceleration.x}, y: {Input.acceleration.y}, z: {Input.acceleration.z}");
    //    Debug.Log(Input.gyro.attitude);
    //    phone.transform.rotation = ninetydegx * GyroToUnity(Input.gyro.attitude);
    //}

    //void FindWebCams()
    //{
    //    foreach (var device in WebCamTexture.devices)
    //    {
    //        Debug.Log("Name: " + device.name);
    //    }
    //}

    //void FindMicrophones()
    //{
    //    foreach (var device in Microphone.devices)
    //    {
    //        Debug.Log("Name: " + device);
    //    }
    //}

    //private static Quaternion GyroToUnity(Quaternion q)
    //{
    //    return new Quaternion(q.x, -q.y, q.z, -q.w);
    //}

    private void Start()
    {
        ServiceLocator.RegisterService<LocationService>(new LocationService());
        var location = ServiceLocator.GetService<LocationService>();
    }
}
