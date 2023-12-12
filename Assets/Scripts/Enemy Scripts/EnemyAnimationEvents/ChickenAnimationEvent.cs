using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenAnimationEvent : MonoBehaviour
{
    public Material chickenWingMaterial;

    public Texture2D deathEmission;
    public Texture2D leftIdleEmission;
    public Texture2D rightIdleEmission;
    public Texture2D leftWalkingEmission;
    public Texture2D rightWalkingEmission;
    private static readonly int GlowTex = Shader.PropertyToID("_GlowTex");
    public void DeathEmission()
    {
        chickenWingMaterial.SetTexture(GlowTex, deathEmission);
    }

    public void LeftIdleEmission()
    {
        chickenWingMaterial.SetTexture(GlowTex, leftIdleEmission);
    }

    public void RightIdleEmission()
    {
        chickenWingMaterial.SetTexture(GlowTex, rightIdleEmission);
    }

    public void LeftWalkingEmission()
    {
        chickenWingMaterial.SetTexture(GlowTex, leftWalkingEmission);
    }

    public void RightWalkingEmission()
    {
        chickenWingMaterial.SetTexture(GlowTex, rightWalkingEmission);
    }
}
