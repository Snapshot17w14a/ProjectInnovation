using UnityEngine;

public class LevelSelector : MonoBehaviour
{
    private BattleManager battleManager;
    [SerializeField] private UIManager uiManager;

    void Start()
    {
        battleManager = FindObjectOfType<BattleManager>();
    }

    public void SetBattleContainer(Battle container)
    {
        battleManager.SetBattleContainer(container);
        uiManager.CloseAllOverlays();
    }
}
