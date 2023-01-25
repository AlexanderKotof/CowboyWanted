using System;
using UnityEngine;

public class ShootingSystem : IDisposable
{
    [Serializable]
    public class Settings
    {
        public float shootingRate = 1f;
        public float shootingDistance = 5f;
        public BulletComponent bulletPrefab;
    }

    private PlayerComponent _player;
    private Settings _settings;

    private ObjectPool<BulletComponent> _bulletsPool;

    private float _lastShootTime;

    public ShootingSystem (PlayerComponent player, Settings settings)
    {
        _player = player;
        _settings = settings;

        _bulletsPool = new ObjectPool<BulletComponent>(_settings.bulletPrefab, null, 3);
    }

    public void Update(CharacterComponent nearestEnemy)
    {
        var direction = nearestEnemy.transform.position - _player.transform.position;

        if (direction.sqrMagnitude > _settings.shootingDistance * _settings.shootingDistance)
            return;

        _player.transform.rotation = Quaternion.Lerp(_player.transform.rotation, Quaternion.LookRotation(direction, Vector3.up), 20 * Time.deltaTime);

        if (_lastShootTime + _settings.shootingRate > Time.realtimeSinceStartup)
            return;

        Shoot(nearestEnemy);
    }

    public void ShootPressed()
    {
        _lastShootTime -= _settings.shootingRate / 2;
    }

    private void Shoot(CharacterComponent nearestEnemy)
    {
        _lastShootTime = Time.realtimeSinceStartup;

        var direction = nearestEnemy.transform.position - _player.bulletSpawnPoint.position + Vector3.up;
        direction.Normalize();

        _bulletsPool.Pool().Shoot(_player.bulletSpawnPoint.position, direction);
    }

    public void Dispose()
    {
        _bulletsPool.Dispose();
    }
}

