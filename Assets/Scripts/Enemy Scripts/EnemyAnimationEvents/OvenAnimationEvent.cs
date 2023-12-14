using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OvenAnimationEvent : MonoBehaviour
{
    public Material ovenMaterial;
    public Texture2D idleEmission;
    private static readonly int GlowTex = Shader.PropertyToID("_GlowTex");
    
    
    public void IdleAnimation()
    {
        ovenMaterial.SetTexture(GlowTex, idleEmission);
    }
}


