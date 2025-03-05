using UnityEngine;

public class PetSelector : MonoBehaviour
{
    private static BattleManager battleManager;

    // Start is called before the first frame update
    void Start()
    {
        battleManager = FindAnyObjectByType<BattleManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        transform.GetSiblingIndex();
    }
}
