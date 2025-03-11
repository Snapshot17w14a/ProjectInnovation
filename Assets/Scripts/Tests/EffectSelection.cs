using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectSelection : MonoBehaviour
{
    [SerializeField] private DecorationManager decorationManager;

    public void OnIceButtonClicked()
    {
        Debug.Log("Ice effect selected.");
    }

    public void OnFireButtonClicked()
    {
        Debug.Log("Fire effect selected.");
    }

    public void OnApplyButtonClicked()
    {

    }
}
