using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Cube : MonoBehaviour
{
    public delegate void OnDisableCallback(Cube instance);
    public OnDisableCallback Disable;
    
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void MoveCube(Vector3 position, Vector3 direction, float speed)
    {
        _rigidbody.velocity = Vector3.zero;
        transform.position = position;
        transform.forward = direction;
        _rigidbody.AddForce(direction * speed, ForceMode.VelocityChange);
    }

    private void OnTriggerEnter(Collider other)
    {
        Disable?.Invoke(this);
    }
}
