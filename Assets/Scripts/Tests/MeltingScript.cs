using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeltingScript : MonoBehaviour
{
    Gyroscope gyroscope;
    Quaternion ninetydegx = Quaternion.Euler(-90, 0, 0);

    [SerializeField]
    private float pouringThreshold;

    void Start()
    {
        gyroscope = Input.gyro;
        gyroscope.enabled = true;
    }

    private void Update()
    {
        Debug.Log(Input.gyro.attitude);

        transform.rotation = ninetydegx * GyroToUnity(Input.gyro.attitude);
    }

    private static Quaternion GyroToUnity(Quaternion q)
    {
        return new Quaternion(q.x, -q.y, q.z, -q.w);
    }

    private void CheckPouring()
    {

    }
}
