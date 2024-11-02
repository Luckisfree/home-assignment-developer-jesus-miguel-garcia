using UnityEngine.Assertions;
using UnityEngine;

public class CharacterViewEventHandler
{
    private Animator _animator;

    //Const Params Keys
    private const string _kAnimatorMovementSpeedParam = "movementSpeed";
    private const string _kAnimatorIdleParam = "idle";
    private const string _kAnimatorAttackParam = "attack";
    private const string _kAnimatorAttackAnimationSpeedParam = "attackAnimationSpeed";
    private const string _kAnimatorHitParam = "hit";
    private const string _kAnimatorDeathParam = "die";

    public void Initialize(Animator animator)
    {
        _animator = animator;
        Assert.IsNotNull(_animator, "_animator not found");
    }

    public void OnIdle()
    {
        _animator.SetFloat(_kAnimatorMovementSpeedParam, 0f);
        _animator.SetBool(_kAnimatorIdleParam, true);
        _animator.SetBool(_kAnimatorAttackParam, false);
        _animator.SetBool(_kAnimatorHitParam, false);
    }

    public void OnMoving(float speed)
    {
        _animator.SetBool(_kAnimatorIdleParam, false);
        _animator.SetBool(_kAnimatorAttackParam, false);
        _animator.SetBool(_kAnimatorHitParam, false);
        _animator.SetFloat(_kAnimatorMovementSpeedParam, speed);
    }

    public void OnAttack(float attackAnimationSpeed)
    {
        _animator.SetFloat(_kAnimatorMovementSpeedParam, 0f);
        _animator.SetFloat(_kAnimatorAttackAnimationSpeedParam, attackAnimationSpeed);
        _animator.SetBool(_kAnimatorIdleParam, false);
        _animator.SetBool(_kAnimatorHitParam, false);
        _animator.SetBool(_kAnimatorAttackParam, true);
    }

    public void OnHit()
    {
        _animator.SetFloat(_kAnimatorMovementSpeedParam, 0f);
        _animator.SetBool(_kAnimatorIdleParam, true);
        _animator.SetBool(_kAnimatorAttackParam, false);
        _animator.SetBool(_kAnimatorHitParam, true);
    }

    public void OnDie()
    {
        _animator.SetFloat(_kAnimatorMovementSpeedParam, 0f);
        _animator.SetBool(_kAnimatorIdleParam, false);
        _animator.SetBool(_kAnimatorAttackParam, false);
        _animator.SetBool(_kAnimatorHitParam, false);
        _animator.SetBool(_kAnimatorDeathParam, true);
    }
}
