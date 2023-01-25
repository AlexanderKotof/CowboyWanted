using System;
using UnityEngine;

public class BulletComponent : MonoBehaviour
{
    private Vector3 _direction;

    public float speed = 5;

    public new Rigidbody rigidbody;

    public static event Action<EnemyComponent> HitEnemy;

    public void Shoot(Vector3 position, Vector3 direction)
    {
        gameObject.SetActive(true);

        transform.rotation = Quaternion.LookRotation(direction);
        transform.position = position;
        _direction = direction;
    }

    // Update is called once per frame
    void Update()
    {
        rigidbody.velocity = _direction * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<EnemyComponent>(out var enemy))
        {
            HitEnemy?.Invoke(enemy);
        }

        gameObject.SetActive(false);
    }
}
