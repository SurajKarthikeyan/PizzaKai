using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ShotgunAnimationEvent : MonoBehaviour
{
    [FormerlySerializedAs("tommyRightHand")] [SerializeField] private Material shotgunRightHand;
    [FormerlySerializedAs("tommyLeftHand")] [SerializeField] private Material shotgunLeftHand;
    [SerializeField] private Texture2D shootEmission;
    [SerializeField] private Texture2D idleEmission;
    private static readonly int GlowTex = Shader.PropertyToID("_GlowTex");

    public void IdleGlowMap()
    {
        shotgunRightHand.SetTexture(GlowTex, idleEmission);
        shotgunLeftHand.SetTexture(GlowTex, idleEmission);

    }

    public void ShootingGlowMap()
    {
        shotgunRightHand.SetTexture(GlowTex, shootEmission);
        shotgunLeftHand.SetTexture(GlowTex, shootEmission);
    }
}
