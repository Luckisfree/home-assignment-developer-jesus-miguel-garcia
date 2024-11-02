using System;
using UnityEngine;
using UnityEngine.Assertions;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] float _health = 100f;
    [SerializeField] HealthBarController _healthBar;

    private float _currentHealth;

    public event Action OnHitAction;
    public event Action OnDieAction;

    private CharactersData _charactersData;
    private bool _death = false;

    void Start()
    {
        Assert.IsNotNull(_healthBar, "Health bar reference not set in HealthComponent");
        Reset();
        _charactersData = ServiceLocator.Instance.GetCharactersData();
    }

    private void OnEnable()
    {
        Reset();
    }


    void TryUpdateHealthBar()
    {
        if(_healthBar == null)
        {
            return;
        }
        _healthBar.UpdateHealthBar(_health, _currentHealth);
    }

    void ResetHealthBar()
    {
        if (_healthBar == null)
        {
            return;
        }
        _healthBar.ResetBarView(_health, _currentHealth);
    }

    public bool IsDying()
    {
        return _death;
    }

    public void Reset()
    {
        _currentHealth = _health;
        _death = false;
        ResetHealthBar();
    }

    public void OnDeath()
    {
        _charactersData.RemoveEnemy(GetComponent<CharacterStateController>());
    }

    public void OnHit(float damage)
    {
        if(_death)
        {
            return;
        }
        _currentHealth -= damage;
        OnHitAction?.Invoke();
        _healthBar.UpdateHealthBar(_health, _currentHealth);
        if (_currentHealth <= 0f)
        {
            OnDieAction?.Invoke();
            _death = true;
        }
    }
}
