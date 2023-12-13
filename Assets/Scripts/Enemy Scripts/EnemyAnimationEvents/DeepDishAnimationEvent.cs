using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class DeepDishAnimationEvent : MonoBehaviour
{
    public Material deepDishMaterial;
    public Material copyThis;

    public Texture2D shootRightEmission;
    public Texture2D shootLeftEmission;
    public Texture2D deathEmission;
    public Texture2D leftIdleEmission;
    public Texture2D rightIdleEmission;
    public Texture2D leftWalkingEmission;
    public Texture2D rightWalkingEmission;
    public AudioSource deepDishSource;
    private static readonly int GlowTex = Shader.PropertyToID("_GlowTex");

    private void Awake()
    {
        deepDishMaterial = new Material(copyThis);
        this.gameObject.GetComponent<SpriteRenderer>().material = deepDishMaterial;

    }

    public void ShootRight()
    {
        deepDishMaterial.SetTexture(GlowTex, shootRightEmission);

    }
    
    public void ShootLeft()
    {
        deepDishMaterial.SetTexture(GlowTex, shootLeftEmission);

    }
    public void DeathEmission()
    {
        AudioDictionary.aDict.PlayAudioClipRemote("deepDishDeath", deepDishSource);
        deepDishMaterial.SetTexture(GlowTex, deathEmission);
    }

    public void LeftIdleEmission()
    {
        deepDishMaterial.SetTexture(GlowTex, leftIdleEmission);
    }

    public void RightIdleEmission()
    {
        deepDishMaterial.SetTexture(GlowTex, rightIdleEmission);
    }

    public void LeftWalkingEmission()
    {
        deepDishMaterial.SetTexture(GlowTex, leftWalkingEmission);
    }

    public void RightWalkingEmission()
    {
        deepDishMaterial.SetTexture(GlowTex, rightWalkingEmission);
    }
}
