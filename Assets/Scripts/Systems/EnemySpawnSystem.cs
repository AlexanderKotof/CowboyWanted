using System;
using System.Collections.Generic;
using UnityEngine;
using static WayPointComponent;

public class EnemySpawnSystem : IDisposable
{
    private EnemyComponent[] _spawnEnemies;

    public bool IsSpawning { get; private set; }

    private float _maxEnemiesCount;

    private float _currentSpawnTime;
    private float _lastSpawnTimer;

    private int _spawnCount;
    private int _spawnedCount;

    private SpawnPointComponent[] _spawnPoints;
    public List<CharacterComponent> Enemies { get; private set; }
    private PlayerComponent _player;

    public event Action<EnemyComponent> EnemyDied;

    public EnemySpawnSystem(SpawnPointComponent[] spawnPoints, PlayerComponent player)
    {
        Enemies = new List<CharacterComponent>();
        _spawnPoints = spawnPoints;
        _player = player;
    }

    public EnemyComponent SpawnRandomEnemy(Vector3 position, Quaternion rotation)
    {
        var index = UnityEngine.Random.Range(0, _spawnEnemies.Length);
        var enemy = GameObject.Instantiate(_spawnEnemies[index], position, rotation);

        return enemy;
    }

    public void Update()
    {
        if (!IsSpawning)
            return;

        if (_lastSpawnTimer + _currentSpawnTime > Time.realtimeSinceStartup)
            return;

        if (Enemies.Count >= _maxEnemiesCount)
            return;

        if (_spawnedCount == _spawnCount)
        {
            IsSpawning = false;
            return;
        }


        Spawn();
    }

    public void StartWave(EnemiesWave wave)
    {
        _currentSpawnTime = wave.spawnRate;
        _spawnEnemies = wave.enemies;
        _maxEnemiesCount = wave.maxEnemiesCount;
        _spawnCount = wave.enemiesCount;

        _spawnedCount = 0;
        IsSpawning = true;
    }

    private void Spawn()
    {
        _spawnedCount++;

        _lastSpawnTimer = Time.realtimeSinceStartup;

        var spawnPoint = _spawnPoints[UnityEngine.Random.Range(0, _spawnPoints.Length)];

        var enemy = SpawnRandomEnemy(spawnPoint.GetPosition(), spawnPoint.GetRotation());

        enemy.SetPlayer(_player);

        Enemies.Add(enemy);

        enemy.Died += OnDied;
    }

    private void OnDied(CharacterComponent enemy)
    {
        enemy.Died -= OnDied;

        EnemyDied?.Invoke(enemy as EnemyComponent);

        enemy.enabled = false;
        GameObject.Destroy(enemy.gameObject, 2f);

        Enemies.Remove(enemy);
    }

    public void Dispose()
    {
        EnemyDied = null;
        Enemies.Clear();
    }
}
