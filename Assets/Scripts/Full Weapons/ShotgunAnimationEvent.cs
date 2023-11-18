using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunAnimationEvent : MonoBehaviour
{
    [SerializeField] private Material tommyRightHand;
    [SerializeField] private Material tommyLeftHand;
    [SerializeField] private Texture2D shootEmission;
    [SerializeField] private Texture2D idleEmission;
    private static readonly int GlowTex = Shader.PropertyToID("_GlowTex");

    public void IdleGlowMap()
    {
        tommyRightHand.SetTexture(GlowTex, idleEmission);
        tommyLeftHand.SetTexture(GlowTex, idleEmission);

    }

    public void ShootingGlowMap()
    {
        tommyRightHand.SetTexture(GlowTex, shootEmission);
        tommyLeftHand.SetTexture(GlowTex, shootEmission);
    }
}
