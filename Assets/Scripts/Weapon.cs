using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Transform attackPoint;
    [SerializeField] private GameObject damageObjectPrefab;
    [SerializeField] private GameObject targetTestObject;
    [SerializeField] private AudioClip[] attackSFX;
    [SerializeField] private int damage;
    [SerializeField] private float spread;
    private Weapon_Base weaponBase;
    [SerializeField] float rotationTweak;

    [SerializeField] private float cooldown;
    private bool canAttack = true;

    private void Awake()
    {
        weaponBase = GetComponent<Weapon_Base>();
    }

    public void Attack()
    {
        switch(canAttack)
        {
            case (true):
                StartCoroutine(AttackCooldown());
                weaponBase.PlayAttackAnimation();
                CreateDamageObject();
                PlayAttackSFX();
                break;
            default:
                weaponBase.PlayIdleAnimation();
                break;
        }
    }

    private void CreateDamageObject()
    {
        GameObject obj = Instantiate(damageObjectPrefab, attackPoint.position, attackPoint.rotation) as GameObject;
        DamageObject damObj = obj.AddComponent<DamageObject>();
        damObj.damage = damage;
    }

    private void PlayAttackSFX()
    {
        SFXHandler.PlaySFXStatic(attackSFX[Random.Range(0, attackSFX.Length - 1)], 0.01f);
    }

    private void FixedUpdate()
    {
        if(canAttack)
        {
            weaponBase.PlayIdleAnimation();
        }
    }

    private IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(cooldown);
        canAttack = true;
    }
}
