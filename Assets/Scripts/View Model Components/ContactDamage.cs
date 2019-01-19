using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactDamage : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] float cooldown;
    private bool onCooldown;
    int layerMask;
    [SerializeField] LayerMask[] layersToHit;
    private void Awake()
    {
        layerMask = LayerMaskUtil.GetLayers(layersToHit);
    }

    private void OnCollisionStay(Collision collision)
    {
        if(!onCooldown)
        {
            if (collision.gameObject.layer == 9 || collision.gameObject.layer == 10) // player or friendly layer
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
