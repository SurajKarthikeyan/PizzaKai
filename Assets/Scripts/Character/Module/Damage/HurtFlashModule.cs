using System.Collections;
using NaughtyAttributes;
using UnityEngine;

public class HurtFlashModule : Module
{
    #region Variables
    [Tooltip("The sprite renderer this operates on.")]
    [ReadOnly]
    public SpriteRenderer spriteRenderer;

    private Color originalColor;
    #endregion

    #region Methods
    protected override void OnLinked(Character old)
    {
        if (!spriteRenderer)
        {
            this.RequireComponent(out spriteRenderer);
            originalColor = spriteRenderer.color; 
        }

        if (old)
        {
            Master.onCharacterHurt.RemoveListener(DoHurt);
            Master.onCharacterDeath.RemoveListener(ResetColor);
        }

        Master.onCharacterHurt.AddListener(DoHurt);
        Master.onCharacterDeath.AddListener(ResetColor);
    }

    private void DoHurt(int dmg) => DoHurt();

    public void DoHurt()
    {
        StartCoroutine(Hurt_CR());
    }

    private IEnumerator Hurt_CR()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(GameManager.Instance.damageTickRate / 2);
        spriteRenderer.color = originalColor;
    }

    /// <summary>
    /// Resets <see cref="spriteRenderer"/> color to <see cref="originalColor"/>
    /// in preparation for destruction.
    /// </summary>
    private void ResetColor()
    {
        StopAllCoroutines();
        spriteRenderer.color = originalColor;
    }
    #endregion
}