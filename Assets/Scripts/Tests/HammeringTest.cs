using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammeringTest : CraftingProcess, ICraftingProcess
{
    [SerializeField]
    private List<Transform> edgePoints;

    [SerializeField]
    private List<GameObject> swordParts;

    [SerializeField]
    private Material material;

    [SerializeField]
    private float successThreshold = 0.5f;

    [SerializeField] private int perfectTreshHold = 2;
    [SerializeField] private int goodTreshHold = 2;
    [SerializeField] private int averageTreshHold = 2;


    [SerializeField]
    private int maxAttempts = 20;
    private int currentAttempts = 0;

    public bool IsProcessDone => isProcessDone;

    private bool isProcessDone = false;

    private Weapon weapon;

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
        int closestEdgeIndex = -1;

        for (int i = 0; i < edgePoints.Count - 1; i++)
        {
            Vector2 point1 = edgePoints[i].position;
            Vector2 point2 = edgePoints[i + 1].position;

            Vector2 closest = ClosestPointOnLineSegment(point1, point2, mousePosition);
            float distance = Vector2.Distance(mousePosition, closest);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEdgeIndex = (Vector2.Distance(closest, point1) < Vector2.Distance(closest, point2)) ? i : i + 1;
            }
        }

        if (closestDistance <= successThreshold)
        {
            ChangeMaterial(closestEdgeIndex);
            DestroySprite(closestEdgeIndex);
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

    private int CalculateGrade()
    {
        int remainingAttempts = maxAttempts - currentAttempts;

        if (remainingAttempts >= maxAttempts - perfectTreshHold)
        {
            return 3;
        }
        else if (remainingAttempts >= goodTreshHold)
        {
            return 2;
        }
        else if (remainingAttempts >= averageTreshHold)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    private void ChangeMaterial(int index)
    {
        if (index >= 0 && index < edgePoints.Count)
        {
            Renderer renderer = edgePoints[index].GetComponent<Renderer>();

            if (renderer != null)
            {
                renderer.material = material;
            }
        }
    }

    private void DestroySprite(int index)
    {
        if (index >= 0 && index < swordParts.Count)
        {
            Destroy(swordParts[index]);
            swordParts[index] = null;
        }

        if (AreAllSpritesDestroyed())
        {
            Debug.Log($"{CalculateGrade()}");
            weapon.SetHammerResult(CalculateGrade());
            isProcessDone = true;
        }
    }

    private bool AreAllSpritesDestroyed()
    {
        foreach (GameObject sprite in swordParts)
        {
            if (sprite != null)
            {
                return false;
            }
        }
        return true;
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

    public void StartProcess(ref Weapon item)
    {
        weapon = item;
    }
}
