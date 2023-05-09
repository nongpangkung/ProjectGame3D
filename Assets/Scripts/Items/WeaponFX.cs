using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFX : MonoBehaviour
{
    [Header("Weapon FX")]
    public ParticleSystem normalWeaponTrail;
    //fire weapon trail
    //dark weapon trail
    //lightning weapon trail

    public void PlayWeaponFX()
    {
        normalWeaponTrail.Stop();

        if (normalWeaponTrail.isStopped)
        {
            normalWeaponTrail.Play();
        }
    }
}

