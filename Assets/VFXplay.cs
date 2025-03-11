using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VFXplay : MonoBehaviour
{
    public VisualEffect vfx;
    //public Color secondaryColor;
    //public Vector3 velocity;
    //private VFXEventAttribute eventAttribute;
    public bool activate;

    void Start()
    {
        // Create vfx event attribute object
        // There is no need to recreate this object every frame
        //eventAttribute = vfx.CreateVFXEventAttribute();
        vfx.Stop();
    }

    // Call this method to the send event
    private void PlayVFX()
    {
        // Set event data
        //eventAttribute.SetFloat("size", Random.Range(0f, 1f));
        //eventAttribute.SetVector3("velocity", velocity);
        //Custom attribute: secondaryColor
        //eventAttribute.SetVector3("secondaryColor", new Vector3(secondaryColor.r, secondaryColor.g, secondaryColor.b));

        // Data is copied from eventAttribute, so this object can be used again
        //vfx.SendEvent("OnPlay", eventAttribute);
    }
    void Update()
    {
        if (activate == true)
        {
            vfx.Play();

        }

        if (activate == false)
        {
            vfx.Stop();

        }
    }
}
