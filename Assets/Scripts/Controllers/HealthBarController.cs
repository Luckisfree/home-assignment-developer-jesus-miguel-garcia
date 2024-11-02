using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    [SerializeField] Image _healthBarFillerImage;
    [SerializeField] float _fillerSpeed = 2f;

    private float _targetFill = 1f;
    private Camera _cacheCamera;

    void Start()
    {
        _cacheCamera = Camera.main;
    }

    public void ResetBarView(float maxHealth, float currentHealth)
    {
        _targetFill = currentHealth / maxHealth;
        _healthBarFillerImage.fillAmount = _targetFill;
    }

    public void UpdateHealthBar(float maxHealth, float currentHealth)
    {
        _targetFill = currentHealth / maxHealth;
    }

    void Update()
    {
        _healthBarFillerImage.fillAmount = Mathf.MoveTowards(_healthBarFillerImage.fillAmount, _targetFill, _fillerSpeed * Time.deltaTime);
    }

    void LateUpdate()
    {
        transform.LookAt(transform.position - _cacheCamera.transform.forward);
    }
}
