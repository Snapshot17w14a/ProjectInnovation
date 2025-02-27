using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PotionMixing : MonoBehaviour
{
    [SerializeField]
    private float hitThreshold = 1f;
    [SerializeField]
    private int failThreshold = 1000;
    [SerializeField]
    private int successThreshold = 900;
    private Vector3 lastAcceleration;
    private int currentNumber;

    private MeshRenderer renderer;

    [SerializeField]
    private List<Material> materials;
    void Start()
    {
        renderer = GetComponent<MeshRenderer>();
        lastAcceleration = Input.acceleration;
    }

    void Update()
    {
        Vector3 currentAcceleration = Input.acceleration;
        float accelerationChange = (currentAcceleration - lastAcceleration).magnitude;

        if (accelerationChange > hitThreshold)
        {
            Debug.Log($"{currentNumber}");
            lastAcceleration = currentAcceleration;
            currentNumber++;
        }

        if (currentNumber >= successThreshold && currentNumber < failThreshold)
        {
            renderer.material = materials[0];
        }
        
        if (currentNumber >= failThreshold)
        {
            renderer.material = materials[1];
        }
    }
}
