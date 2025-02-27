using UnityEngine;

public class AssemblyManager : MonoBehaviour
{
    [SerializeField] GameObject[] items;
    private GameObject[] assemblySnapPoints;
    private GameObject weapon;
    int currentSnapIndex = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        weapon = transform.Find("Weapon").gameObject;
        var snapObject = weapon.transform.Find("Snappoints");
        assemblySnapPoints = new GameObject[snapObject.childCount];
        for (int i = 0; i < assemblySnapPoints.Length; i++) 
        {
            assemblySnapPoints[i] = snapObject.GetChild(i).gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddItem(int id)
    {
        Instantiate(items[id], assemblySnapPoints[currentSnapIndex].transform.position, Quaternion.identity, weapon.transform);
        currentSnapIndex++;
    }
}
