using UnityEngine.Assertions;
using UnityEngine;

public class CharacterViewController : MonoBehaviour
{
    [SerializeField] WeaponViewController _weaponViewController;

    private CharacterStateController _characterStateController;
    private CharacterViewEventHandler _characterViewEventHandler = new CharacterViewEventHandler();
    private WeaponType _weaponType = WeaponType.CurvedSword;


    void Awake()
    {
        _characterViewEventHandler.Initialize(GetComponent<Animator>());
        _characterStateController = transform.parent.GetComponent<CharacterStateController>();
        if (!HasValidData())
        {
            Assert.IsFalse(true, "character state controller not assigned on CharacterViewController");
            return;
        }
    }

    private void OnEnable()
    {
        if (_weaponViewController == null)
        {
            return;
        }
        _weaponViewController.SetWeaponView(_weaponType);
    }

    public void SetWeaponType(WeaponType weaponType)
    {
        _weaponType = weaponType;
        _weaponViewController.SetWeaponView(_weaponType);
    }

    bool HasValidData()
    {
        return _characterStateController != null;
    }

    public void OnIdle()
    {
        _characterViewEventHandler.OnIdle();
    }

    public void OnMoving(float speed)
    {
        _characterViewEventHandler.OnMoving(speed);
    }

    public void OnAttack(float attackAnimationSpeed)
    {
        _characterViewEventHandler.OnAttack(attackAnimationSpeed);
    }

    void OnAttackEvent()
    {
        if(!HasValidData())
        {
            return;
        }

        _characterStateController.OnAttackEvent();
    }

    public void OnHitAction()
    {
        _characterViewEventHandler.OnHit();
    }

    void OnHitExecuted()
    {
        _characterViewEventHandler.OnIdle();
    }

    public void OnDieAction()
    {
        _characterViewEventHandler.OnDie();
    }

    void OnDeath()
    {
        _characterStateController.OnDeath();
    }

}
