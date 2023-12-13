using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForkySoloAnimationEvents : MonoBehaviour
{
    public Material forkyMaterial;

    public Texture2D forkyIdleEmission;
    public Texture2D forkyCheekedUpEmission;
    private static readonly int GlowTex = Shader.PropertyToID("_GlowTex");
    public GameObject TouchGrass0;
    public GameObject TouchGrass1;


    public void IdleEmission()
    {
        forkyMaterial.SetTexture(GlowTex, forkyIdleEmission);
    }

    public void CheekedUpEmission()
    {
        forkyMaterial.SetTexture(GlowTex, forkyCheekedUpEmission);
        TouchGrass0.SetActive(false);
        TouchGrass1.SetActive(false);
    }
}
