using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyContactDamage : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] float cooldown;
    private bool onCooldown;

    private void OnCollisionEnter(Collision collision)
    {
        if(!onCooldown)
        {
            if (collision.gameObject.layer == 9) // player layer
            {
                Health collisionHP = collision.gameObject.GetComponent<Health>();

                if (collisionHP != null)
                    collisionHP.ModifyHP(-damage);

                StartCoroutine(DamageCooldown());
            }
        }
    }

    private IEnumerator DamageCooldown()
    {
        onCooldown = true;
        yield return new WaitForSeconds(cooldown);
        onCooldown = false;
    }
}
