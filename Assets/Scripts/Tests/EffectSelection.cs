using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectSelection : MonoBehaviour
{
    [SerializeField] private DecorationManager decorationManager;

    public void OnIceButtonClicked()
    {
        EffectHolder.SelectedEffect = new IceEffect();
        Debug.Log("Ice effect selected.");
    }

    public void OnFireButtonClicked()
    {
        EffectHolder.SelectedEffect = new FireEffect();
        Debug.Log("Fire effect selected.");
    }

    public void OnApplyButtonClicked()
    {
        if (EffectHolder.SelectedEffect != null)
        {
            decorationManager.CompleteProcess(EffectHolder.SelectedEffect);
            Debug.Log($"Effect {EffectHolder.SelectedEffect.GetType().Name} applied!");
        }
    }
}
