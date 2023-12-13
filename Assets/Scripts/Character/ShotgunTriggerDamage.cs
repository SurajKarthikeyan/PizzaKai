using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunTriggerDamage : MonoBehaviour
{
    private CharacterMovementModule character;
    [SerializeField]
    private ShotGunWeapon shotgun;

    // Start is called before the first frame update
    void Start()
    {
        character = transform.GetComponentInParent<CharacterMovementModule>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && character.isShotgunDashing)
        {
            EnemyBasic enemy = collision.gameObject.GetComponent<EnemyBasic>();
            if (enemy != null)
            {
                if (enemy.currentHP <= shotgun.dashDamage)
                {
                    shotgun.dashReset = true;
                    shotgun.altFireDelay.Finish();
                }
                enemy.TakeDamage(shotgun.dashDamage);
            }
        }
    }
}
