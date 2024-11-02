using UnityEngine;

public class CharacterRotationController : MonoBehaviour
{
    [SerializeField] float _rotationSpeed = 720f;

    private Vector3 _rotation = Vector3.zero;
    private Quaternion _rotationQuaternion = Quaternion.identity;

    public void RotateTowards(Vector3 rotation)
    {
        _rotation = rotation;
    }

    // Update is called once per frame
    void Update()
    {
        _rotationQuaternion = _rotation == Vector3.zero ? Quaternion.identity : Quaternion.LookRotation(_rotation, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, _rotationQuaternion, _rotationSpeed * Time.deltaTime);
    }
}
