using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PouringVialView : MonoBehaviour
{
    [SerializeField]
    private PouringMetal vial;

    [SerializeField]
    private RectTransform pointA;
    [SerializeField]
    private RectTransform pointB;

    // Start is called before the first frame update
    void Awake()
    {
        if (vial == null)
        {
            throw new NullReferenceException(nameof(vial));
        }

        if (pointA == null)
        {
            throw new NullReferenceException(nameof(pointA));
        }

        if (pointB == null)
        {
            throw new NullReferenceException(nameof(pointB));
        }

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(pointA.position, pointB.position, vial.MoveFraction);
    }
}
