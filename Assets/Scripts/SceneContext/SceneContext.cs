using UnityEngine;

public class SceneContext : MonoBehaviour
{
    public SpawnPointComponent playerSpawnPoint;

    public SpawnPointComponent[] enemySpawnPoints;

    public WayPointComponent[] wayPoints;

    public CameraRigComponent cameraRig;

    public static SceneContext instance;

    private void Awake()
    {
        instance = this;
    }
}
