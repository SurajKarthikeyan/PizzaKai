using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BreadstickAnimationEvent : MonoBehaviour
{
    public Material copyThis;
    public Material breadStickMaterial;

    public Texture2D deathEmission;
    public Texture2D leftIdleEmission;
    public Texture2D leftWalkingEmission;
    public Texture2D rightWalkingEmission;
    public Texture2D jumpingUpEmission;
    private static readonly int GlowTex = Shader.PropertyToID("_GlowTex");

    private void Awake()
    {
        breadStickMaterial = new Material(copyThis);
        this.gameObject.GetComponent<SpriteRenderer>().material = breadStickMaterial;
    }


    public void DeathEmission()
    {
        breadStickMaterial.SetTexture(GlowTex, deathEmission);
        Debug.Log("Switched to death");
    }

    public void JumpingEmission()
    {
        breadStickMaterial.SetTexture(GlowTex, jumpingUpEmission);
        Debug.Log("Switched to jump");

    }
    
    public void LeftIdleEmission()
    {
        breadStickMaterial.SetTexture(GlowTex, leftIdleEmission);
        Debug.Log("Switched to idle");

    }
    

    public void LeftWalkingEmission()
    {
        breadStickMaterial.SetTexture(GlowTex, leftWalkingEmission);
        Debug.Log("Switched to left walk");

    }

    public void RightWalkingEmission()
    {
        breadStickMaterial.SetTexture(GlowTex, rightWalkingEmission);
        Debug.Log("Switched to right walk");

    }
}
