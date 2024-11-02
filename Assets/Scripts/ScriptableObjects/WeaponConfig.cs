using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponConfig", menuName = "ScriptableObjects/WeaponConfig")]
public class WeaponConfig : ScriptableObject
{
    [SerializeField] float _attackAnimationSpeed = 1f;
    [SerializeField] float _attackInterval = 0.5f;
    [SerializeField] float _movementSpeed = 0.5f;
    [SerializeField] float _attackRange = 2.5f;
    [SerializeField] float _attackDamage = 50f;

    public float AttackAnimationSpeed()
    { 
        return _attackAnimationSpeed; 
    }
    public float AttackInterval()
    {
        return _attackInterval;
    }

    public float MovementSpeed()
    {
        return _movementSpeed;
    }

    public float AttackRange()
    {
        return _attackRange;
    }
    public float AttackDamage()
    {
        return _attackDamage;
    }
}
