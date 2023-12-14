using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorAnimationEvents : MonoBehaviour
{
    public Material copyThis;
    public Material generatorMaterial;
    public Texture2D idleEmission;
    public Texture2D damagedEmission;
    private static readonly int GlowTex = Shader.PropertyToID("_GlowTex");

    private void Awake()
    {
        generatorMaterial = new Material(copyThis);
        this.gameObject.GetComponent<SpriteRenderer>().material = generatorMaterial;
    }

    public void IdleEmission()
    {
        generatorMaterial.SetTexture(GlowTex, idleEmission);
    }

    public void DamagedEmission()
    {
        generatorMaterial.SetTexture(GlowTex, damagedEmission);
    }
}
