using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamethrowerAnimationEvent : MonoBehaviour
{
    [SerializeField] private Material flamethrowerHand;
    [SerializeField] private Texture2D shootIdleEmission;
    [SerializeField] private Texture2D reloadEmission;
    private static readonly int GlowTex = Shader.PropertyToID("_GlowTex");


    public void IdleShoot()
    {
        flamethrowerHand.SetTexture(GlowTex, shootIdleEmission);
    }

    public void Reload()
    {
        flamethrowerHand.SetTexture(GlowTex, reloadEmission);
    }
}
