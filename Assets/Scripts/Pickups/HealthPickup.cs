using System.Collections;
using UnityEngine;

public class HealthPickup : Pickup
{
    #region Variables
    public int healthIncrease;

    private float existence;
    [Tooltip("How longer this lasts before disappearing in seconds.")]
    public float lifetime;

    public SpriteRenderer sRender;
    private float flickerInterval = 0;
    
    #endregion

    #region Pickup Implementation
    protected override bool ReceiveCharacter(Character character)
    {
        if (character.HP == character.maxHP)
            return false;
        AudioDictionary.aDict.PlayAudioClip("healthPickup", AudioDictionary.Source.Pickup);
        character.Heal(healthIncrease);
        return true;
    }
    #endregion

    private void Update()
    {
        existence += Time.deltaTime;
        if (existence >= lifetime)
        {
            Destroy(gameObject);
        }
        if (existence >= lifetime * 0.85f)
        {
            if (flickerInterval <= 0)
                Flicker();
            else
                flickerInterval -= Time.deltaTime;
        }
    }

    void UnFlicker()
    {
        sRender.enabled = true;
    }
    void Flicker()
    {
        sRender.enabled = false;
        Invoke("UnFlicker", 0.1f);
        flickerInterval = 0.3f;
        
    }
}