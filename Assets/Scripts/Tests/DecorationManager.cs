using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

public class DecorationManager : CraftingProcess, ICraftingProcess
{
    [SerializeField] private Button iceEffectButton;
    [SerializeField] private Button fireEffectButton;
    [SerializeField] private Button waterEffectButton;
    [SerializeField] private Button thunderEffectButton;
    [SerializeField] private Button leafEffectButton;
    [SerializeField] private Button poisonEffectButton;

    [SerializeField] private GameObject iceVFXPrefab;
    [SerializeField] private GameObject fireVFXPrefab;
    [SerializeField] private GameObject waterVFXPrefab;
    [SerializeField] private GameObject thunderVFXPrefab;
    [SerializeField] private GameObject leafVFXPrefab;
    [SerializeField] private GameObject poisonVFXPrefab;

    private GameObject currentVFX;

    public bool IsProcessDone => isProcessDone;

    private Weapon weapon;

    private bool isProcessDone = false;

    private IWeaponEffect currentWeaponEffect;

    [SerializeField] private Transform effectPoint;


    private void Awake()
    {
        if (iceEffectButton == null)
        {
            throw new NullReferenceException(nameof(iceEffectButton));
        }

        iceEffectButton.onClick.AddListener(OnIceEffectButtonClicked);
        fireEffectButton.onClick.AddListener(OnFireEffectButtonClicked);
        waterEffectButton.onClick.AddListener(OnWaterEffectButtonClicked);
        thunderEffectButton.onClick.AddListener(OnThunderEffectButtonClicked);
        leafEffectButton.onClick.AddListener(OnLeafEffectButtonClicked);
        poisonEffectButton.onClick.AddListener(OnPoisonEffectButtonClicked);
    }

    private void SpawnVFX(GameObject vfxPrefab)
    {
        if (currentVFX != null)
        {
            Destroy(currentVFX);
        }

        currentVFX = Instantiate(vfxPrefab, effectPoint.position, effectPoint.rotation, effectPoint);
    }

    public void CompleteProcess()
    {
        if (currentWeaponEffect != null)
        {
            weapon.SetDecorationResult(currentWeaponEffect);
            isProcessDone = true;
        }
    }

    public void StartProcess(ref Weapon item)
    {
        weapon = item;
    }

    private void OnIceEffectButtonClicked()
    {
        currentWeaponEffect = new IceEffect();
        SpawnVFX(iceVFXPrefab);
    }

    private void OnFireEffectButtonClicked()
    {
        currentWeaponEffect = new FireEffect();
        SpawnVFX(fireVFXPrefab);
    }

    private void OnWaterEffectButtonClicked()
    {
        currentWeaponEffect = new WaterEffect();
        SpawnVFX(waterVFXPrefab);
    }

    private void OnThunderEffectButtonClicked()
    {
        currentWeaponEffect = new ThunderEffect();
        SpawnVFX(thunderVFXPrefab);
    }

    private void OnLeafEffectButtonClicked()
    {
        currentWeaponEffect = new LeafEffect();
        SpawnVFX(leafVFXPrefab);
    }

    private void OnPoisonEffectButtonClicked()
    {
        currentWeaponEffect = new PoisonEffect();
        SpawnVFX(poisonVFXPrefab);
    }
}
