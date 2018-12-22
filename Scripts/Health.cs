using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] int maxHP;
    [SerializeField] bool isPlayer;
    private int HP;
    private SpriteRenderer spriteRenderer;
    private bool isInvuln;

    private void Awake()
    {
        HP = maxHP;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private IEnumerator DamageFlashTemp() // DEMO CODE
    {
        if (spriteRenderer == null)
            yield return null;

        isInvuln = true;
        spriteRenderer.material.color = Color.red;
        yield return new WaitForSeconds(0.05f);
        spriteRenderer.material.color = Color.white;
        yield return new WaitForSeconds(0.05f);
        spriteRenderer.material.color = Color.red;
        yield return new WaitForSeconds(0.05f);
        spriteRenderer.material.color = Color.white;
        yield return new WaitForSeconds(0.05f);
        spriteRenderer.material.color = Color.red;
        yield return new WaitForSeconds(0.05f);
        spriteRenderer.material.color = Color.white;
        yield return null;
        isInvuln = false;

    }

    public void ModifyHP(int modifyValue)
    {
        if(!isInvuln)
        {
            HP += modifyValue;
            StartCoroutine(DamageFlashTemp());
            if (HP <= 0)
            {
                if(!isPlayer)
                {
                    // DIE
                    Destroy(gameObject);
                    return;
                }
                
                // player death stuff, maybe better in a new class
                Debug.Log("Player HP: " + HP);
            }

        }
    }
}
