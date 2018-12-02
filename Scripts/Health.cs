using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] int maxHP;
    private int HP;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        HP = maxHP;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private IEnumerator DamageFlashTemp() // DEMO CODE
    {
        if (spriteRenderer == null)
            yield return null;

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

    }

    public void ModifyHP(int modifyValue)
    {
        HP += modifyValue;
        StartCoroutine(DamageFlashTemp());
        if(HP <= 0)
        {
            // DIE
            Destroy(gameObject);
        }
    }
}
