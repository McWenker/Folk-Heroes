using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private enum WeaponState
    {
        Idle,
        Attack
    }
    private WeaponState state;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private GameObject damageObjectPrefab;
    [SerializeField] private GameObject targetTestObject;
    [SerializeField] private AudioClip[] attackSFX;
    [SerializeField] private int damage;
    [SerializeField] private int spreadCount;
    [SerializeField] private float spreadAmount;
    private Weapon_Base weaponBase;
    [SerializeField] float rotationTweak;

    [SerializeField] private float cooldown;
    private bool canAttack = true;

    private void Awake()
    {
        if(spreadCount == 0)
        {
            spreadCount = 1;
        }
        weaponBase = GetComponent<Weapon_Base>();
    }

    public void Attack()
    {
        switch(canAttack)
        {
            case (true):
                canAttack = false;
                CreateDamageObjects();
                PlayAttackSFX();
                state = WeaponState.Attack;
                break;
            default:
                break;
        }
    }

    private void CreateDamageObjects()
    {
        Vector3 startingAttackRotation = attackPoint.rotation.eulerAngles;
        for(int i = 0; i < spreadCount; ++i)
        {
            Quaternion attackAngle = Quaternion.Euler(new Vector3(startingAttackRotation.x, startingAttackRotation.y + (Random.Range(-spreadAmount, spreadAmount)), startingAttackRotation.z));
            GameObject obj = Instantiate(damageObjectPrefab, attackPoint.position, attackAngle) as GameObject;
            IDamage damObj = obj.GetComponent<IDamage>();
            damObj.Damage = damage;
            damObj.CreatorWeapon = this;
            damObj.AttackOrigin = attackPoint;
        }
    }

    private void PlayAttackSFX()
    {
        SFXHandler.PlaySFXStatic(attackSFX[Random.Range(0, attackSFX.Length - 1)], 0.01f);
    }

    private void FixedUpdate()
    {
        switch(state)
        {
            case WeaponState.Idle:                
                weaponBase.PlayIdleAnimation();
                break;
            case WeaponState.Attack:                
                weaponBase.PlayAttackAnimation(() => StartCoroutine(AttackCooldown()));
                break;
        }
    }

    private IEnumerator AttackCooldown()
    {
        state = WeaponState.Idle;
        yield return new WaitForSeconds(cooldown);
        canAttack = true;
    }
}
