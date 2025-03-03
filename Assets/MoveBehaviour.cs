using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBehaviour : MonoBehaviour
{
    [SerializeField] private float speed = 5f;

    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float move = (Input.acceleration.x / speed ) * Time.deltaTime;
        float newX = transform.position.x + move;

        newX = Mathf.Clamp(newX, pointA.position.x, pointB.position.x);
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }
}
