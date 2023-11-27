using UnityEngine;

/// <summary>
/// A handle to be used by animator to destroy the gameobjects.
/// For example, this is used by Explosion to get rid of the
/// latent gameobject once the explosion is over.
/// 
/// <br/>
/// 
/// For games with a lot more explosions, ObjectPools
/// <see cref="https://docs.unity3d.com/ScriptReference/Pool.ObjectPool_1.html"/>
/// would be more appropriate.
/// 
/// <br/>
/// 
/// Authors: Ryan Chang (2023)
/// </summary>
public class DestroyHandle : MonoBehaviour
{
    public void DestroyGameObject()
    {
        Destroy(gameObject);
    }
}