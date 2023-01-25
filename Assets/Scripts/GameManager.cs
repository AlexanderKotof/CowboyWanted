using System.Collections;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PlayerComponent playerPrefab;

    public PlayerComponent Player { get; private set; }

    private SceneContext _sceneContext;

    private EnemySpawnSystem _enemySpawner;

    public ShootingSystem.Settings shootingSettings;
    private ShootingSystem _shootingManager;

    public WeaponSystem.Settings weaponSettings;
    private WeaponSystem _weaponSystem;

    public BombSystem.Settings bombSettings;
    private BombSystem _bombSystem;

    private CoinsSystem _coinsSystem;

    private View _gameView;
    private GameController _controller;
    private float _minDistance = 0.1f;

    private int _currentWayPoint;

    private bool _gameStarted;

    private void Start()
    {
        DontDestroyOnLoad(this);
        GameSceneLoader.LoadGameScene(OnGameSceneLoaded);
    }

    private void OnGameSceneLoaded()
    {
        ScreenSystem.ScreensManager.ShowScreen<StartGameScreen>().SetStartCallback(StartGame);
    }

    private void StartGame()
    {
        _currentWayPoint = 0;
        _sceneContext = SceneContext.instance;

        InstatiatePlayer();

        SetupSystems();

        SetupController();

        SetupGameScreen();

        Subscribe();

        WayPointReached(_sceneContext.wayPoints[_currentWayPoint]);

        _gameStarted = true;
    }

    private void Subscribe()
    {
        _enemySpawner.EnemyDied += OnEnemyDied;
        BulletComponent.HitEnemy += OnHitEnemy;
        Player.Died += OnPlayerDied;

        _coinsSystem.CoinsUpdated += _gameView.coins.SetValue;
        _weaponSystem.WeaponUpgraded += OnWeaponUpgraded;
        _bombSystem.CooldownUpdated += OnBombCooldownUpdated;
    }

    private void Unsubscribe()
    {
        _coinsSystem.CoinsUpdated -= _gameView.coins.SetValue;
        _weaponSystem.WeaponUpgraded -= OnWeaponUpgraded;
        _bombSystem.CooldownUpdated -= OnBombCooldownUpdated;

        _enemySpawner.EnemyDied -= OnEnemyDied;
        BulletComponent.HitEnemy -= OnHitEnemy;
        Player.Died -= OnPlayerDied;
    }

    private void OnPlayerDied(CharacterComponent obj)
    {
        _gameStarted = false;

        StopAllCoroutines();

        ScreenSystem.ScreensManager.HideAll();
        ScreenSystem.ScreensManager.ShowScreen<GameOverScreen>().SetRestartCallback(RestartGame);
    }

    private void OnBombCooldownUpdated(int obj)
    {
        _gameView.bombCooldown.Value = obj;
    }

    private void OnWeaponUpgraded()
    {
        _gameView.pistolLevel.Value = _weaponSystem.GetWeaponLevel();
        _gameView.upgradePrice.Value = _weaponSystem.GetUpgradeCost();
    }

    private void SetupSystems()
    {
        _sceneContext.cameraRig.player = Player;

        _enemySpawner = new EnemySpawnSystem(_sceneContext.enemySpawnPoints, Player);
        _shootingManager = new ShootingSystem(Player, shootingSettings);
        _coinsSystem = new CoinsSystem();
        _weaponSystem = new WeaponSystem(weaponSettings, _coinsSystem);
        _bombSystem = new BombSystem(bombSettings, Player);
    }

    private void OnEnemyDied(EnemyComponent enemy)
    {
        _coinsSystem.AddCoins(enemy.reward);
    }

    private IEnumerator MoveToWaypoint(WayPointComponent waypoint)
    {
        Player.MoveTo(waypoint.transform.position);

        while ((Player.transform.position - waypoint.transform.position).sqrMagnitude > _minDistance * _minDistance)
        {
            yield return null;
        }

        WayPointReached(waypoint);
    }

    private void WayPointReached(WayPointComponent waypoint)
    {
        if (waypoint.enemiesWave.enemiesCount == 0)
        {
            MoveToNext();
        }
        else
        {
            SpawnEnemies(waypoint);
        }
    }

    private void SpawnEnemies(WayPointComponent waypoint)
    {
        StartCoroutine(WaitForWaveIsOver(waypoint));
    }

    private IEnumerator WaitForWaveIsOver(WayPointComponent waypoint)
    {
        _enemySpawner.StartWave(waypoint.enemiesWave);

        while (_enemySpawner.IsSpawning || _enemySpawner.Enemies.Count > 0)
        {
            yield return null;
        }

        MoveToNext();
    }

    private void MoveToNext()
    {
        _currentWayPoint++;

        if (_sceneContext.wayPoints.Length > _currentWayPoint)
            StartCoroutine(MoveToWaypoint(_sceneContext.wayPoints[_currentWayPoint]));
        else
            LevelEnded();
    }

    private void LevelEnded()
    {
        _gameStarted = false;

        StopAllCoroutines();

        ScreenSystem.ScreensManager.HideAll();
        ScreenSystem.ScreensManager.ShowScreen<LevelEndedScreen>().SetRestartCallback(RestartGame);
    }

    private void RestartGame()
    {
        Unsubscribe();

        ScreenSystem.ScreensManager.HideAll();
        GameSceneLoader.LoadGameScene(OnGameSceneLoaded);
    }

    private void SetupGameScreen()
    {
        ScreenSystem.ScreensManager.HideAll();

        _gameView = new View();
        var screen = ScreenSystem.ScreensManager.ShowScreen<GameScreen>();
        screen.SetInfo(_controller, _gameView);

        _gameView.upgradePrice.Value = _weaponSystem.GetUpgradeCost();
        _gameView.pistolLevel.Value = _weaponSystem.GetWeaponLevel();
        _gameView.bombCooldown.Value = _bombSystem.GetCooldown();
        _gameView.coins.Value = _coinsSystem.Coins;
    }

    private void InstatiatePlayer()
    {
        Player = Instantiate(playerPrefab, _sceneContext.playerSpawnPoint.GetPosition(), _sceneContext.playerSpawnPoint.GetRotation());
    }

    private void SetupController()
    {
        _controller = new GameController();
        _controller.shoot += _shootingManager.ShootPressed;
        _controller.upgradeWeapon += _weaponSystem.UpgradeWeapon;
        _controller.throwBomb += _bombSystem.ThrowBomb;
    }

    private void OnDestroy()
    {
        Unsubscribe();

        _gameView.Dispose();

        _coinsSystem.Dispose();
        _shootingManager.Dispose();
        _enemySpawner.Dispose();
        _controller.Dispose();
        _bombSystem.Dispose();
    }

    private void OnHitEnemy(EnemyComponent enemy)
    {
        enemy.TakeDamage(_weaponSystem.Weapon.damage);
    }

    private void Update()
    {
        if (!_gameStarted)
                return;

        _enemySpawner.Update();

        if (_enemySpawner.Enemies.Count == 0)
            return;

        var nearestEnemy = _enemySpawner.Enemies.OrderBy(obj => Vector3.SqrMagnitude(obj.transform.position - Player.transform.position)).First();

        _shootingManager.Update(nearestEnemy);
    }
}
