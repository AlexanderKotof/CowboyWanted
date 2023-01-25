using System;
using System.Collections;
using UnityEngine;

public class BombSystem : IDisposable
{
    [Serializable]
    public class Settings
    {
        public BombComponent bombPrefab;

        public ExplosionComponent explosion;

        public float damageRadius;

        public float damage;

        public float throwRange;

        public float explodeAfter;

        public int cooldown;
    }

    private ObjectPool<BombComponent> _bombPool;
    private ObjectPool<ExplosionComponent> _explosionPool;

    private Settings _settings;
    private PlayerComponent _player;

    private int _cooldownLeft;
    public event Action<int> CooldownUpdated;

    public BombSystem(Settings settings, PlayerComponent player)
    {
        _settings = settings;
        _bombPool = new ObjectPool<BombComponent>(_settings.bombPrefab, null);
        _player = player;
        _explosionPool = new ObjectPool<ExplosionComponent>(_settings.explosion, null);

        BombComponent.BombExplode += OnBombExplode;
    }

    private void OnBombExplode(BombComponent obj)
    {
        Collider[] results = new Collider[20];
        Physics.OverlapSphereNonAlloc(obj.transform.position, _settings.damageRadius, results);

        for (int i = 0; i < results.Length; i++)
        {
            if (results[i] == null)
                continue;

            if (!results[i].TryGetComponent<EnemyComponent>(out var enemy))
                continue;

            enemy.TakeDamage(_settings.damage);
        }

        var explosion = _explosionPool.Pool();
        explosion.transform.position = obj.transform.position;
        explosion.gameObject.SetActive(true);
    }

    public void ThrowBomb()
    {
        if (_cooldownLeft > 0)
            return;

        _cooldownLeft = _settings.cooldown;

        var bomb = _bombPool.Pool();
        var direction = _player.transform.forward + Vector3.up;
        bomb.Throw(direction.normalized, _player.bulletSpawnPoint.position, _settings.throwRange, _settings.explodeAfter);

        _player.StartCoroutine(BombCooldown());
    }

    private IEnumerator BombCooldown()
    {
        CooldownUpdated?.Invoke(_cooldownLeft);

        while(_cooldownLeft > 0)
        {
            yield return new WaitForSeconds(1);
            _cooldownLeft--;
            CooldownUpdated?.Invoke(_cooldownLeft);
        }
    }

    public int GetCooldown()
    {
        return _cooldownLeft;
    }

    public void Dispose()
    {
        BombComponent.BombExplode -= OnBombExplode;
        _bombPool.Dispose();
    }
}
