using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class BattleManager : MonoBehaviour
{
    [SerializeField] private Battle battleContainer;

    private bool isBattleInProgress = false;
    private int currentWaveIndex = 0;

    private readonly Enemy[] enemiesInBattle = new Enemy[5];
    private readonly Pet[] petsInBattle = new Pet[3];

    private readonly Vector2[] enemyPositions = new Vector2[5];
    private Transform enemyParent;

    private readonly Vector2[] petPositions = new Vector2[3];
    private Transform petParent;

    [SerializeField] private GameObject[] objectsToHideInBattle;

    [SerializeField] private Button fightButton;

    public bool IsAttackingAllowed => isBattleInProgress;
    public Enemy[] AllEnemies => enemiesInBattle;

    [SerializeField] private DecorationManager decorationManager;

    [SerializeField] private SoundEffectPlayer soundPlayer;

    private int EnemiesAlive
    {
        get
        {
            int count = 0;
            foreach (var enemy in enemiesInBattle) if (enemy != null) count++;
            return count;
        }
    }

    private bool AreTherePetsInBattle
    {
        get
        {
            foreach(var pet in petsInBattle) if (pet != null) return true;
            return false;
        }
    }

    public Action OnBattleEnd;

    // Start is called before the first frame update
    void Start()
    {
        Transform characterParet = transform.Find("Characters");
        petParent = characterParet.Find("Pets");
        enemyParent = characterParet.Find("Enemies");

        //for(int i = 0; i < 3; i++) if (petParent.GetChild(i).TryGetComponent<Pet>(out petsInBattle[i])) petsInBattle[i].SetManager(this);
        for (int i = 0; i < 3; i++)
        {
            var petTransform = petParent.GetChild(i);
            petPositions[i] = petTransform.position;
            Destroy(petTransform.gameObject);
        }
        for (int i = 0; i < 5; i++)
        {
            var enemyTransform = enemyParent.GetChild(i);
            enemyPositions[i] = enemyTransform.position;
            Destroy(enemyTransform.gameObject);
        }

        Skill.battleManager = this;
        fightButton.interactable = AreTherePetsInBattle;
    }

    private IEnumerator StartWaves()
    {
        //Debug.Log("Coroutine started");
        soundPlayer.PlayAudioAtIndex(1);

        while (currentWaveIndex < battleContainer.enemyWaves.Length)
        {
            battleContainer.enemyWaves[currentWaveIndex].Initialize();
            //Debug.Log($"Wave index: {currentWaveIndex}, number of enemies: {enemyWaves[currentWaveIndex].waveEnemies.Length}");

            for (int i = 0; i < battleContainer.enemyWaves[currentWaveIndex].waveEnemies.Length; i++)
            {
                CreateEnemy(battleContainer.enemyWaves[currentWaveIndex].waveEnemies[i], i);
            }

            isBattleInProgress = true;

            //Debug.Log($"Battle started, waiting for enemies to die");
            yield return new WaitUntil(() => EnemiesAlive == 0);
            //Debug.Log($"Battle ended, enemies dead");

            isBattleInProgress = false;
            currentWaveIndex++;
        }

        //Debug.Log($"All waves beat");
        OnBattleEnd?.Invoke();
        yield return null;
    }

    private void Update()
    {
        if (!isBattleInProgress) return;
        if (!AreTherePetsInBattle) OnBattleEnd?.Invoke();
    }

    public void StartBattle()
    {
        foreach (var gameObject in objectsToHideInBattle) gameObject.SetActive(false);
        foreach (var pet in petsInBattle) if (pet != null) pet.StartBattle();
        OnBattleEnd += BattleEnd;
        StartCoroutine(StartWaves());
    }

    public Character GetTargetCharacter(Character.CharacterType type)
    {
        int i = -1;
        if (type == Character.CharacterType.Enemy)
        {
            while (i + 1 < petsInBattle.Length && petsInBattle[++i] == null) { }
            return petsInBattle[i];
        }
        else if (type == Character.CharacterType.Pet)
        {
            while (i + 1 < enemiesInBattle.Length && enemiesInBattle[++i] == null) { }
            return enemiesInBattle[i];
        }
        return null;
    }

    private void CreateEnemy(Enemy enemy, int slotIndex)
    {
        if (enemy == null) return;
        //Debug.Log($"Creating enemy with index: {slotIndex}");
        enemiesInBattle[slotIndex] = Instantiate<Enemy>(enemy, enemyPositions[slotIndex], Quaternion.identity, enemyParent);
        enemiesInBattle[slotIndex].SetManager(this);
        enemiesInBattle[slotIndex].StartBattle();
    }

    public void SetPetAtIndex(Pet pet, int index)
    {
        if (pet == null)
        {
            petsInBattle[index] = null;
            return;
        }
        if (petsInBattle[index] != null) Destroy(petsInBattle[index].gameObject);
        petsInBattle[index] = Instantiate<Pet>(pet, Vector2.zero, Quaternion.identity, transform);
        petsInBattle[index].transform.SetParent(petParent);
        petsInBattle[index].transform.SetSiblingIndex(index);
        petsInBattle[index].transform.position = petPositions[index];
        petsInBattle[index].SetManager(this);
        fightButton.interactable = AreTherePetsInBattle;
    }

    private void BattleEnd()
    {
        soundPlayer.PlayAudioAtIndex(0);
        foreach (var obj in objectsToHideInBattle) obj.SetActive(true);
        currentWaveIndex = 0;
        StopAllCoroutines();
        foreach (var pet in petsInBattle) if (pet != null) pet.StopAllCoroutines();
        if (!AreTherePetsInBattle)
        {
            for (int i = enemiesInBattle.Length - 1; i >= 0; i--)
            {
                if (enemiesInBattle[i] == null) continue;
                Destroy(enemiesInBattle[i].gameObject);
                enemiesInBattle[i] = null;
            }
        }
        isBattleInProgress = false;
        fightButton.interactable = AreTherePetsInBattle;
        OnBattleEnd -= BattleEnd;
    }

    public void RemovePetFromBattle(Pet pet)
    {
        for(int i = 0; i < petsInBattle.Length; i++)
        {
            if (petsInBattle[i] == null) continue;
            else if (petsInBattle[i] == pet) petsInBattle[i] = null;
        }
    }

    public void RemoveEnemyFromBattle(Enemy enemy)
    {
        for (int i = 0; i < enemiesInBattle.Length; i++)
        {
            if (enemiesInBattle[i] == null) continue;
            else if (enemiesInBattle[i] == enemy) enemiesInBattle[i] = null;
        }
    }

    public void SetBattleContainer(Battle container)
    {
        battleContainer = container;
    }
}
