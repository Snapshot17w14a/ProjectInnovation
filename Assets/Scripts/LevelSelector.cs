using TMPro;
using UnityEngine;

public class LevelSelector : MonoBehaviour
{
    private BattleManager battleManager;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private TextMeshProUGUI levelNumber;

    void Start()
    {
        battleManager = FindObjectOfType<BattleManager>();
    }

    public void SetBattleContainer(Battle container)
    {
        battleManager.SetBattleContainer(container);
        levelNumber.text = container.name;
        uiManager.CloseAllOverlays();
    }
}
