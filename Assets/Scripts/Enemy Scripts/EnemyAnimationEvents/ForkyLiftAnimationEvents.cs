using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForkyLiftAnimationEvents : MonoBehaviour
{
    public Material forkyLiftMaterial;

    public Texture2D forkyLiftIdleEmission;
    private static readonly int GlowTex = Shader.PropertyToID("_GlowTex");

    public void IdleEmission()
    {
        forkyLiftMaterial.SetTexture(GlowTex, forkyLiftIdleEmission);

    }
}
