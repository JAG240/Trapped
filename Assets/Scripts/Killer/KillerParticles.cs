using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillerParticles : MonoBehaviour
{
    [SerializeField] ParticleSystem bloodSplat;

    public void ChopParticle()
    {
        bloodSplat.Emit(60);
    }
}
