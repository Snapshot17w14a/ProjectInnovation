using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectSelection : MonoBehaviour
{
    [SerializeField] private DecorationManager decorationManager;

    private IWeaponEffect selectedEffect;

    public void OnIceButtonClicked()
    {
        EffectHolder.SelectedEffect = new IceEffect();
        Debug.Log("Ice effect selected.");
    }

    public void OnFireButtonClicked()
    {
        //selectedEffect = new FireEffect();
        //Debug.Log("Fire effect selected.");
    }

    public void OnApplyButtonClicked()
    {
        if (EffectHolder.SelectedEffect != null)
        {
            decorationManager.CompleteProcess(selectedEffect);
            Debug.Log($"Effect {selectedEffect.GetType().Name} applied!");
        }
    }
}
