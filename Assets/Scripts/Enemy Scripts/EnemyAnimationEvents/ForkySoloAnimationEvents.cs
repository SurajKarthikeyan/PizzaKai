using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForkySoloAnimationEvents : MonoBehaviour
{
    public Material forkyMaterial;

    public Texture2D forkyIdleEmission;
    public Texture2D forkyCheekedUpEmission;
    private static readonly int GlowTex = Shader.PropertyToID("_GlowTex");


    public void IdleEmission()
    {
        forkyMaterial.SetTexture(GlowTex, forkyIdleEmission);
    }

    public void CheekedUpEmission()
    {
        forkyMaterial.SetTexture(GlowTex, forkyCheekedUpEmission);
    }
}
