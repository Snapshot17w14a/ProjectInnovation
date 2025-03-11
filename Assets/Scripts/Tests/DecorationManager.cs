using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DecorationManager : CraftingProcess, ICraftingProcess
{
    [SerializeField]
    private Button iceEffectButton;



    public bool IsProcessDone => isProcessDone;

    private Weapon weapon;

    private bool isProcessDone = false;


    private void Awake()
    {
        if (iceEffectButton == null)
        {
            throw new NullReferenceException(nameof(iceEffectButton));
        }








        iceEffectButton.onClick.AddListener(OnIceEffectButtonClicked);

    }

    public void CompleteProcess(IWeaponEffect newEffect)
    {
        weapon.SetDecorationResult(newEffect);
        isProcessDone = true;
    }

    public void StartProcess(ref Weapon item)
    {
        weapon = item;
    }

    private void OnIceEffectButtonClicked()
    {
        CompleteProcess(new IceEffect());
    }

}
