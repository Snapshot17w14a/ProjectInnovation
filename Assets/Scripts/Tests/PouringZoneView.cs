using System;
using UnityEngine;

public class PouringZoneView : MonoBehaviour
{
    [SerializeField]
    private PouringZone pouringZone;

    [SerializeField]
    private RectTransform pointA;
    [SerializeField] 
    private RectTransform pointB;

    private void Awake()
    {
        if (pouringZone == null)
        {
            throw new NullReferenceException(nameof(pouringZone));
        }
    }

    private void Update()
    {
        float y = Mathf.Lerp(pointA.position.y, pointB.position.y, pouringZone.PourFraction);
        transform.position = new Vector3(transform.position.x, y, transform.position.z);
    }
}
