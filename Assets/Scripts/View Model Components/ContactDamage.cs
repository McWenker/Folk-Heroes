using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactDamage : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] float cooldown;
    private bool onCooldown;
    [SerializeField] LayerMask layersToHit;

    private void OnCollisionStay(Collision collision)
    {
        if(!onCooldown)
        {
            if (LayerMaskUtil.CheckLayerMask(layersToHit, collision.gameObject.layer)) // player or friendly layer
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
