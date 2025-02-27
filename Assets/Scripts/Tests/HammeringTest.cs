using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammeringTest : MonoBehaviour
{
    [SerializeField]
    private List<Transform> edgePoints;

    [SerializeField]
    private Material material;

    [SerializeField]
    private float successThreshold = 0.5f;

    [SerializeField]
    private int maxAttempts = 20;
    private int currentAttempts = 0;


    void Start()
    {

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            CheckHit(mousePosition);
        }
    }

    private void CheckHit(Vector2 mousePosition)
    {
        float closestDistance = float.MaxValue;
        Transform closestEdge = null;


        for (int i = 0; i < edgePoints.Count - 1; i++)
        {
            Vector2 point1 = edgePoints[i].position;
            Vector2 point2 = edgePoints[i + 1].position;

            Vector2 closest = ClosestPointOnLineSegment(point1, point2, mousePosition);
            float distance = Vector2.Distance(mousePosition, closest);

            if (distance < closestDistance)
            {
                closestDistance = distance;

                closestEdge = (Vector2.Distance(closest, point1) < Vector2.Distance(closest, point2))? edgePoints[i]: edgePoints[i + 1];
            }
        }

        if (closestDistance <= successThreshold)
        {
            ChangeMaterial(closestEdge);
        }
        else
        {
            currentAttempts++;
            Debug.Log($"Miss! Attempts Left: {maxAttempts - currentAttempts}");
        }

        if (currentAttempts >= maxAttempts)
        {
            Debug.LogError("Out of Attempts!");
        }
    }

    private void ChangeMaterial(Transform target)
    {
        Renderer renderer = target.GetComponent<Renderer>();

        if (renderer != null)
        {
            renderer.material = material;
        }
    }

    private Vector2 ClosestPointOnLineSegment(Vector2 A, Vector2 B, Vector2 mousePosition)
    {
        Vector2 AB = B - A;
        Vector2 AMP = mousePosition - A;

        float LenghtAB = AB.magnitude;
        float ABdotAMP = Vector2.Dot(AMP, AB);
        float distance = ABdotAMP / LenghtAB;

        distance = Mathf.Clamp01(distance);

        return A + distance * AB;
    }
}
