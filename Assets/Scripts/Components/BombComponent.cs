using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombComponent : MonoBehaviour
{
    public Rigidbody rigidbody;

    private Vector3 _direction;

    private float _explodeAfter;
    private float _startTime;
    private bool exploded;

    public static event Action<BombComponent> BombExplode;

    public void Throw(Vector3 direction, Vector3 position, float force, float explodeAfter)
    {
        transform.position = position;
        _direction = direction;

        gameObject.SetActive(true);

        _explodeAfter = explodeAfter;

        _startTime = Time.realtimeSinceStartup;

        rigidbody.AddForce(_direction * force, ForceMode.Impulse);

        exploded = false;
    }

    private void Update()
    {
        if (_startTime + _explodeAfter < Time.realtimeSinceStartup)
            Explode();
    }

    private void Explode()
    {
        if (exploded)
            return;

        exploded = true;
        BombExplode?.Invoke(this);

        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<EnemyComponent>(out var _))
            Explode();
    }
}
